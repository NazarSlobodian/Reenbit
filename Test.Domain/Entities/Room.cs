namespace Test.Domain.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }
}
