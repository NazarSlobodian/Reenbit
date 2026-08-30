using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Test.Application.Interfaces.Services;

namespace Test.Presentation.Controllers
{
    /// <summary>
    /// Manages meeting room bookings.
    /// </summary>
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingManagementService _bookingService;

        public BookingsController(IBookingManagementService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Books a specific available time slot for the current user.
        /// </summary>
        [HttpPost("{timeSlotId:guid}")]
        public async Task<IActionResult> Book(Guid timeSlotId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _bookingService.CreateBookingAsync(timeSlotId, userId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns the current user's own bookings.
        /// </summary>
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _bookingService.GetMyBookingsAsync(userId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Admin only: returns every booking across all users.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _bookingService.GetAllBookingsAsync(cancellationToken);
            return Ok(result);
        }
    }
}