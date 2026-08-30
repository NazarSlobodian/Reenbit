namespace Test.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid TimeSlotId { get; set; }
        public string UserId { get; set; } = string.Empty;

        public TimeSlot TimeSlot { get; set; } = null!;
    }
}