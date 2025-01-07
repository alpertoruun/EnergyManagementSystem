using System.Net;
using System.Net.Mail;
using EnergyManagementSystem.Core.Configuration;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace EnergyManagementSystem.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.UseTls,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.Username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var subject = "Password Reset Request";
            var body = $@"
        <h2>Password Reset Request</h2>
        <p>To reset your password, click the link below:</p>
        <p><a href='{_emailSettings.ClientUrl}/reset-password?token={resetToken}&email={to}'>Reset Password</a></p>
        <p>If you didn't request a password reset, please ignore this email.</p>
        <p>This link will expire in 1 hour.</p>";
            await SendEmailAsync(to, subject, body);
        }
        public async Task SendEmailConfirmationAsync(string to, string token)
        {
            var subject = "Email Doğrulama";
            var body = $@"
        <h2>Hesap Doğrulama</h2>
        <p>Hesabınızı doğrulamak için aşağıdaki linke tıklayın:</p>
        <p><a href='{_emailSettings.ClientUrl}/verify-email?token={token}&email={to}'>Hesabı Doğrula</a></p>
        <p>Bu linki siz istemediyseniz, lütfen bu emaili görmezden gelin.</p>
        <p>Bu link 1 saat içinde geçerliliğini yitirecektir.</p>";
            await SendEmailAsync(to, subject, body);
        }
        public async Task SendEmailChangeConfirmationAsync(string to, string token)
        {
            var subject = "Email Change Confirmation";
            var body = $@"
        <h2>Email Change Request</h2>
        <p>To confirm your new email address, click the link below:</p>
        <p><a href='{_emailSettings.ClientUrl}/email-confirmation?token={token}&email={to}'>Confirm Email Change</a></p>
        <p>If you didn't request this change, please ignore this email.</p>
        <p>This link will expire in 1 hour.</p>";
            await SendEmailAsync(to, subject, body);
        }
    }
}