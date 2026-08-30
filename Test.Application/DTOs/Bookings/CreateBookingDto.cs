namespace Test.Application.DTOs.Bookings
{
    public record CreateBookingDto(Guid RoomId, DateTime StartTime, DateTime EndTime, List<Guid> SelectedServiceIds);
}
