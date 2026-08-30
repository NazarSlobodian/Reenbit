using Test.Domain.Entities;

namespace Test.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity, CancellationToken cancellationToken = default);

        Task AddAsync(Room room, CancellationToken cancellationToken = default);
        Task UpdateAsync(Room room, CancellationToken cancellationToken = default);
        Task DeleteAsync(Room room, CancellationToken cancellationToken = default);
    }
}
