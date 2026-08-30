using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces.Services;

namespace Test.Presentation.Controllers
{
    /// <summary>
    /// Manages meeting rooms and their schedules.
    /// </summary>
    [ApiController]
    [Route("api/rooms")]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomManagementService _roomService;

        public RoomsController(IRoomManagementService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Admin only: creates a new meeting room. Its time-slot grid is generated automatically.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRoomDto dto, CancellationToken cancellationToken)
        {
            var id = await _roomService.CreateRoomAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
        }

        /// <summary>
        /// Admin only: updates an existing room's details.
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateRoomDto dto, CancellationToken cancellationToken)
        {
            await _roomService.UpdateRoomAsync(id, dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Admin only: soft-deletes a room. Existing bookings and time slot history are preserved.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _roomService.DeleteRoomAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Lists all active rooms.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetAllRoomsAsync(cancellationToken);
            return Ok(rooms);
        }

        /// <summary>
        /// Gets a single room by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            return Ok(room);
        }

        /// <summary>
        /// Returns a room's time slots (free/booked) within a date range.
        /// </summary>
        [HttpGet("{id:guid}/schedule")]
        public async Task<IActionResult> GetSchedule(Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken cancellationToken)
        {
            var timeSlots = await _roomService.GetScheduleAsync(id, from, to, cancellationToken);
            return Ok(timeSlots);
        }
    }
}