namespace Test.Application.DTOs.RoomManagement
{
    public record UpdateRoomDto(string Name, int Capacity, decimal BasePricePerHour, List<UpdateRoomServiceDto> Services);
}
