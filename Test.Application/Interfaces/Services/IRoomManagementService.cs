using Test.Application.DTOs.RoomManagement;

namespace Test.Application.Interfaces.Services
{
    public interface IRoomManagementService
    {
        Task<Guid> CreateRoomAsync(CreateRoomDto dto, CancellationToken cancellationToken = default);
        Task UpdateRoomAsync(Guid id, UpdateRoomDto dto, CancellationToken cancellationToken = default);
        Task DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default);
        Task<RoomDto> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSlotDto>> GetScheduleAsync(Guid roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}