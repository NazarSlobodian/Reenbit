using Test.Domain.Entities;

namespace Test.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}
