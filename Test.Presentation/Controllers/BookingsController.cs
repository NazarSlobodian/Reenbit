using Microsoft.AspNetCore.Mvc;

using Test.Application.DTOs.Bookings;
using Test.Application.Interfaces.Services;

namespace Test.Presentation.Controllers
{
    /// <summary>
    /// Manages conference room bookings.
    /// </summary>
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingManagementService _bookingService;

        public BookingsController(IBookingManagementService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Creates a new booking and calculates the final price based on the selected time and services.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Book(CreateBookingDto dto)
        {
            var result = await _bookingService.CreateBookingAsync(dto);
            return Ok(result);
        }
    }
}
