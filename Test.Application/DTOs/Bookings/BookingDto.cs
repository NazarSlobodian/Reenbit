namespace Test.Application.DTOs.Bookings
{
    public record BookingDto(Guid Id, Guid TimeSlotId, Guid RoomId, DateTime StartTime, DateTime EndTime, string UserId);
}