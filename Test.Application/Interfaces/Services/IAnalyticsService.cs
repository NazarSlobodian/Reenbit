using Test.Application.DTOs.Analytics;

namespace Test.Application.Interfaces.Services
{
    public interface IAnalyticsService
    {
        Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}
