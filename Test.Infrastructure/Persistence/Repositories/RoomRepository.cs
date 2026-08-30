using Microsoft.EntityFrameworkCore;

using Test.Domain.Entities;

using Test.Application.Interfaces.Repositories;

namespace Test.Infrastructure.Persistence.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Services)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Services)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Services)
                .Where(r => r.Capacity >= capacity)
                .Where(r => !r.Bookings.Any(b =>
                    b.StartTime < endTime && b.EndTime > startTime))
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Room room, CancellationToken cancellationToken = default)
        {
            await _context.Rooms.AddAsync(room, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Room room, CancellationToken cancellationToken = default)
        {
            room.IsDeleted = true;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
