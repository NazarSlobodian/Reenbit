namespace Test.Domain.Entities
{
    public class TimeSlot : BaseEntity
    {
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSlotStatus Status { get; set; } = TimeSlotStatus.Available;

        public byte[] RowVersion { get; set; } = null!;

        public Room Room { get; set; } = null!;
        public Booking? Booking { get; set; }
    }
}
