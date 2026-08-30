using Test.Application.DTOs.Bookings;

namespace Test.Application.Interfaces.Services
{
    public interface IBookingManagementService
    {
        Task<BookingDto> CreateBookingAsync(Guid timeSlotId, string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookingDto>> GetMyBookingsAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default); 
    }
}