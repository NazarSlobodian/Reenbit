using Test.Domain.Entities;

namespace Test.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IEnumerable<Booking>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}