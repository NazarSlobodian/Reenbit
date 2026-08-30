namespace Test.Domain.Entities
{
    public class BookingService : BaseEntity
    {
        public Guid BookingId { get; set; }
        public Guid RoomServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;
        public decimal PriceAtBooking { get; set; }

        public Booking Booking { get; set; } = null!;
        public RoomService RoomService { get; set; } = null!;
    }
}
