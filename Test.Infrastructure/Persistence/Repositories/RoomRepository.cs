using Microsoft.EntityFrameworkCore;

using Test.Application.Interfaces.Repositories;
using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context) => _context = context;

        public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Rooms.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Room room, CancellationToken cancellationToken = default)
        {
            await _context.Rooms.AddAsync(room, cancellationToken);
        }

        public Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
        {
            _context.Rooms.Update(room);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Room room, CancellationToken cancellationToken = default)
        {
            room.IsDeleted = true;
            _context.Rooms.Update(room);
            return Task.CompletedTask;
        }
    }
}