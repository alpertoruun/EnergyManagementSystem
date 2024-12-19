using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.DTOs.Notification;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.DTOs.Schedule;

public class ScheduleBackgroundJob
{
    private readonly IScheduleService _scheduleService;
    private readonly IDeviceService _deviceService;
    private readonly INotificationService _notificationService;
    private readonly string _logPath;


    public ScheduleBackgroundJob(
        IScheduleService scheduleService,
        IDeviceService deviceService,
        INotificationService notificationService)
    {
        _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
    }



    private void Log(string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        Console.WriteLine(logMessage);
        File.AppendAllText(_logPath, logMessage + Environment.NewLine);
    }

    public async Task Process()
    {
        try
        {
            var currentTime = DateTime.UtcNow;
            Log($"Job başladı: {currentTime}");

            IEnumerable<DeviceDto> devices;
            try
            {
                devices = await _deviceService.GetAllDevicesAsync();
                Log($"Toplam {devices.Count()} cihaz bulundu");
            }
            catch (Exception ex)
            {
                Log($"Cihazları getirirken hata: {ex.Message}");
                throw;
            }

            foreach (var device in devices)
            {
                try
                {
                    Log($"Device kontrolü: {device.DeviceId}-{device.Name}");
                    IEnumerable<ScheduleDto> activeSchedules;

                    try
                    {
                        activeSchedules = await _scheduleService.GetActiveSchedulesAsync(device.DeviceId);
                        Log($"Cihaz için {activeSchedules.Count()} aktif schedule bulundu");
                    }
                    catch (Exception ex)
                    {
                        Log($"Schedule'ları getirirken hata: {ex.Message}");
                        Log($"Hata detayı: {ex.StackTrace}");
                        continue;
                    }

                    foreach (var schedule in activeSchedules)
                    {
                        Log($"Schedule işlenmeye başlıyor - ID:{schedule.ScheduleId}");
                        try
                        {
                            Log($"Schedule kontrolü - ID:{schedule.ScheduleId}, Start:{schedule.StartTime}, End:{schedule.EndTime}, CurrentTime:{currentTime}");

                            if (string.IsNullOrEmpty(schedule.Repeat))
                            {
                                Log($"Tek seferlik schedule kontrol ediliyor...");

                                try
                                {
                                    var startMatch = IsTimeToExecute(schedule.StartTime, currentTime);
                                    if (startMatch)
                                    {
                                        Log($"Tek seferlik schedule START işlemi başlıyor...");
                                        await _deviceService.UpdateDeviceStatusAsync(device.DeviceId, true);
                                        await CreateNotification(device, $"{device.Name} isimli cihaz program doğrultusunda açıldı.");
                                        Log($"Tek seferlik schedule START işlemi tamamlandı.");
                                    }

                                    var endMatch = IsTimeToExecute(schedule.EndTime, currentTime);
                                    if (endMatch)
                                    {
                                        Log($"Tek seferlik schedule END işlemi başlıyor...");
                                        await _deviceService.UpdateDeviceStatusAsync(device.DeviceId, false);
                                        await CreateNotification(device, $"{device.Name} isimli cihaz program doğrultusunda kapatıldı.");

                                        Log($"Schedule silme işlemi başlıyor...");
                                        try
                                        {
                                            await _scheduleService.DeleteAsync(schedule.ScheduleId);
                                            await CreateNotification(device, $"{device.Name} isimli cihazın tek seferlik programı tamamlandı ve silindi.");
                                            Log($"Schedule silme işlemi tamamlandı.");
                                        }
                                        catch (Exception ex)
                                        {
                                            Log($"Schedule silme işlemi sırasında hata: {ex.Message}");
                                            Log($"Hata detayı: {ex.StackTrace}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log($"Schedule işlem sırasında hata: {ex.Message}");
                                    Log($"Hata detayı: {ex.StackTrace}");
                                }
                            }
                            Log($"Schedule ID:{schedule.ScheduleId} işlemi tamamlandı.");
                        }
                        catch (Exception scheduleEx)
                        {
                            Log($"Schedule genel işlem hatası: {scheduleEx.Message}");
                            Log($"Hata detayı: {scheduleEx.StackTrace}");
                        }
                    }
                    Log($"Device ID:{device.DeviceId} işlemleri tamamlandı.");
                }
                catch (Exception deviceEx)
                {
                    Log($"Device işlemi sırasında hata: {deviceEx.Message}");
                    Log($"Hata detayı: {deviceEx.StackTrace}");
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Genel hata: {ex.Message}");
            Log($"Hata detayı: {ex.StackTrace}");
            throw;
        }
    }

    private bool IsTimeToExecute(DateTime scheduleTime, DateTime currentTime)
    {
        Log($"Zaman karşılaştırması:");
        Log($"Schedule Time: {scheduleTime:yyyy-MM-dd HH:mm}");
        Log($"Current Time:  {currentTime:yyyy-MM-dd HH:mm}");

        var scheduleTimeNormalized = new DateTime(
            scheduleTime.Year,
            scheduleTime.Month,
            scheduleTime.Day,
            scheduleTime.Hour,
            scheduleTime.Minute,
            0,
            DateTimeKind.Utc
        );

        var currentTimeNormalized = new DateTime(
            currentTime.Year,
            currentTime.Month,
            currentTime.Day,
            currentTime.Hour,
            currentTime.Minute,
            0,
            DateTimeKind.Utc
        );

        var timeDifference = scheduleTimeNormalized - currentTimeNormalized;
        var isMatch = Math.Abs(timeDifference.TotalMinutes) < 1;

        Log($"Zaman farkı (dakika): {timeDifference.TotalMinutes}");
        Log($"Karşılaştırma sonucu: {isMatch}");

        return isMatch;
    }


    private bool IsRecurringTimeMatch(DateTime scheduleTime, DateTime currentTime, string repeat)
    {
        var currentDay = currentTime.DayOfWeek.ToString().ToLower();
        if (!repeat.ToLower().Contains(currentDay))
            return false;

        return scheduleTime.Hour == currentTime.Hour &&
               scheduleTime.Minute == currentTime.Minute;
    }

    private async Task CreateNotification(DeviceDto device, string message)
    {
        var userId = await _deviceService.GetUserIdByDeviceIdAsync(device.DeviceId);
        if (userId == null)
        {
            throw new Exception("UserId could not be found for the given device.");
        }

        var notification = new CreateNotificationDto
        {
            UserId = userId.Value,
            Message = message,
            Type = "Schedule"
        };

        await _notificationService.CreateAsync(notification);
    }
}