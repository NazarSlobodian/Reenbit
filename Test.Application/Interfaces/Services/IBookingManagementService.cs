using Test.Application.DTOs.Bookings;

namespace Test.Application.Interfaces.Services
{
    public interface IBookingManagementService
    {
        Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    }
}
