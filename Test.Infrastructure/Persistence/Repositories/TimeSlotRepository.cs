using Microsoft.EntityFrameworkCore;
using Test.Application.Interfaces.Repositories;
using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeSlotRepository(ApplicationDbContext context) => _context = context;

        public async Task<TimeSlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.TimeSlots
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TimeSlot>> GetByRoomAsync(Guid roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _context.TimeSlots
                .Where(s => s.RoomId == roomId && s.StartTime >= from && s.StartTime < to)
                .OrderBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TimeSlot> timeSlots, CancellationToken cancellationToken = default)
        {
            await _context.TimeSlots.AddRangeAsync(timeSlots, cancellationToken);
        }
    }
}