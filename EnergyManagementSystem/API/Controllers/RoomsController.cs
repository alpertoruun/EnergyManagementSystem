using EnergyManagementSystem.Core.DTOs.Room;
using EnergyManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        return await _roomService.GetByIdAsync(id);
    }

    [HttpGet("house/{houseId}")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetHouseRooms(int houseId)
    {
        return Ok(await _roomService.GetHouseRoomsAsync(houseId));
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(CreateRoomDto createRoomDto)
    {
        var room = await _roomService.CreateAsync(createRoomDto);
        return CreatedAtAction(nameof(GetById), new { id = room.RoomId }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRoomDto updateRoomDto)
    {
        await _roomService.UpdateAsync(id, updateRoomDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roomService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/devices")]
    public async Task<ActionResult<RoomDto>> GetRoomWithDevices(int id)
    {
        return await _roomService.GetRoomWithDevicesAsync(id);
    }
}