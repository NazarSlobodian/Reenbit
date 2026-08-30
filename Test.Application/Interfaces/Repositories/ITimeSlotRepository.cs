using Test.Domain.Entities;

namespace Test.Application.Interfaces.Repositories
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<TimeSlot>> GetByRoomAsync(Guid roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TimeSlot> timeSlots, CancellationToken cancellationToken = default);
    }
}