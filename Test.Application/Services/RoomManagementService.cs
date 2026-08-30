using Microsoft.Extensions.Options;

using Test.Application.Common;
using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;
using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly ITimeSlotGenerator _timeSlotGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SlotGenerationOptions _slotOptions;

        public RoomManagementService(
            IRoomRepository roomRepository,
            ITimeSlotRepository timeSlotRepository,
            ITimeSlotGenerator timeSlotGenerator,
            IUnitOfWork unitOfWork,
            IOptions<SlotGenerationOptions> slotOptions)
        {
            _roomRepository = roomRepository;
            _timeSlotRepository = timeSlotRepository;
            _timeSlotGenerator = timeSlotGenerator;
            _unitOfWork = unitOfWork;
            _slotOptions = slotOptions.Value;
        }

        public async Task<Guid> CreateRoomAsync(CreateRoomDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Room name cannot be empty.");

            var room = new Room { Name = dto.Name };
            await _roomRepository.AddAsync(room, cancellationToken);

            var timeSlots = _timeSlotGenerator.GenerateForRoom(room.Id, DateTime.UtcNow.Date, _slotOptions);
            await _timeSlotRepository.AddRangeAsync(timeSlots, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return room.Id;
        }

        public async Task UpdateRoomAsync(Guid id, UpdateRoomDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Room name cannot be empty.");

            var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {id} was not found.");

            room.Name = dto.Name;

            await _roomRepository.UpdateAsync(room, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {id} was not found.");

            await _roomRepository.DeleteAsync(room, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default)
        {
            var rooms = await _roomRepository.GetAllAsync(cancellationToken);
            return rooms.Select(r => new RoomDto(r.Id, r.Name));
        }

        public async Task<RoomDto> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {id} was not found.");
            return new RoomDto(room.Id, room.Name);
        }

        public async Task<IEnumerable<TimeSlotDto>> GetScheduleAsync(Guid roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            var timeSlots = await _timeSlotRepository.GetByRoomAsync(roomId, from, to, cancellationToken);
            return timeSlots.Select(s => new TimeSlotDto(s.Id, s.StartTime, s.EndTime, s.Status));
        }
    }
}