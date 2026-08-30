using Test.Application.DTOs.Analytics;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;

namespace Test.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookingRepository _bookingRepository;

        public AnalyticsService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            if (from == default || to == default)
                throw new ArgumentException("Both 'from' and 'to' dates must be provided.");

            if (from >= to)
                throw new ArgumentException("'from' date must be earlier than 'to' date.");

            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(from, to, cancellationToken);

            return new RevenueReportDto(
                TotalBookings: bookings.Count(),
                RoomRevenue: bookings.Sum(b => b.RoomPriceTotal),
                ServicesRevenue: bookings.Sum(b => b.ServicesPriceTotal),
                TotalRevenue: bookings.Sum(b => b.TotalPrice)
            );
        }
    }
}