namespace Test.Domain.Entities
{
    public class RoomService : BaseEntity
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public Room? Room { get; set; }
    }
}
