using Microsoft.AspNetCore.Mvc;

using Test.Application.Interfaces.Services;

namespace Test.Presentation.Controllers
{
    /// <summary>
    /// Provides business analytics and financial reports.
    /// </summary>
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Generates a revenue report for a specified date range.
        /// </summary>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var report = await _analyticsService.GetRevenueReportAsync(from, to);
            return Ok(report);
        }
    }
}
