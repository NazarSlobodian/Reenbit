namespace Test.Application.DTOs.Bookings
{
    public record MyBookingDto(
        Guid Id,
        Guid RoomId,
        string RoomName,
        DateTime StartTime,
        DateTime EndTime
    );
}