namespace Test.Application.DTOs.RoomManagement
{
    public record CreateRoomDto(string Name, int Capacity, decimal BasePricePerHour, List<CreateRoomServiceDto> Services);
}
