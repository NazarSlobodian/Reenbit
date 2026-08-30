using Microsoft.EntityFrameworkCore;
using Test.Application.Interfaces.Repositories;

using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("The room was just booked or modified by another user. Please try again.");
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Where(b => b.StartTime >= from && b.EndTime <= to)
                .ToListAsync(cancellationToken);
        }
    }
}
