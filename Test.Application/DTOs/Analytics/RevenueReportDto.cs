namespace Test.Application.DTOs.Analytics
{
    public record RevenueReportDto(
        int TotalBookings,
        decimal RoomRevenue,
        decimal ServicesRevenue,
        decimal TotalRevenue
    );
}
