using Microsoft.AspNetCore.Mvc;

using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces.Services;

namespace Test.Presentation.Controllers
{
    /// <summary>
    /// Manages conference rooms and searches for availability.
    /// </summary>
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomManagementService _roomService;

        public RoomsController(IRoomManagementService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Creates a new conference room with base pricing and available services.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            var id = await _roomService.CreateRoomAsync(dto);
            return CreatedAtAction(nameof(Search), new { id }, new { Id = id });
        }

        /// <summary>
        /// Updates an existing room and synchronizes its available services.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateRoomDto dto)
        {
            await _roomService.UpdateRoomAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Soft deletes a conference room by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Searches for available rooms that meet the capacity and time requirements.
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> Search([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int capacity)
        {
            var rooms = await _roomService.SearchAvailableRoomsAsync(start, end, capacity);
            return Ok(rooms);
        }
    }
}
