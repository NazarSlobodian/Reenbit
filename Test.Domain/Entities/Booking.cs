namespace Test.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal RoomPriceTotal { get; set; }
        public decimal ServicesPriceTotal { get; set; }
        public decimal TotalPrice { get; set; }

        public Room? Room { get; set; }
        public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
    }
}
