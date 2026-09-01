namespace Test.Presentation.Hubs.Clients
{
    public interface IBookingClient
    {
        Task TimeSlotStatusChanged(Guid timeSlotId, int newStatus);
    }
}