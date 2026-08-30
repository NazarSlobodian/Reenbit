using Test.Application.DTOs.RoomManagement;

namespace Test.Application.Interfaces.Services
{
    public interface IRoomManagementService
    {
        Task<Guid> CreateRoomAsync(CreateRoomDto dto, CancellationToken cancellationToken = default);
        Task UpdateRoomAsync(Guid id, UpdateRoomDto dto, CancellationToken cancellationToken = default);
        Task DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoomDto>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity, CancellationToken cancellationToken = default);
    }
}
