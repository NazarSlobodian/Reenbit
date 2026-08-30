using Microsoft.EntityFrameworkCore;
using Test.Application.Interfaces.Repositories;
using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .IgnoreQueryFilters()
                .Include(b => b.TimeSlot)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.TimeSlot.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.TimeSlot)
                .OrderBy(b => b.TimeSlot.StartTime)
                .ToListAsync(cancellationToken);
        }
    }
}