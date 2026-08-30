namespace Test.Domain.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }
        public bool IsDeleted { get; set; } = false;

        public byte[] RowVersion { get; set; } = null!;

        public ICollection<RoomService> Services { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
