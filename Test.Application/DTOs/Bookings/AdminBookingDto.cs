namespace Test.Application.DTOs.Bookings
{
    public record AdminBookingDto(
        Guid Id,
        Guid RoomId,
        string RoomName,
        DateTime StartTime,
        DateTime EndTime,
        string UserId
    );
}