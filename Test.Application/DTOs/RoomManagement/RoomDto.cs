namespace Test.Application.DTOs.RoomManagement
{
    public record RoomDto(Guid Id, string Name, int Capacity, decimal BasePricePerHour, List<RoomServiceDto> Services);
}
