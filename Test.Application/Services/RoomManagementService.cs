using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;

using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomManagementService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Guid> CreateRoomAsync(CreateRoomDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Room name cannot be empty.");
            if (dto.Capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");
            if (dto.BasePricePerHour < 0)
                throw new ArgumentException("Price cannot be negative.");

            var room = new Room
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                BasePricePerHour = dto.BasePricePerHour,
                Services = dto.Services.Select(s => new RoomService
                {
                    Name = s.Name,
                    Price = s.Price
                }).ToList()
            };

            await _roomRepository.AddAsync(room, cancellationToken);
            return room.Id;
        }

        public async Task UpdateRoomAsync(Guid id, UpdateRoomDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Room name cannot be empty.");
            if (dto.Capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");
            if (dto.BasePricePerHour < 0)
                throw new ArgumentException("Price cannot be negative.");

            var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {id} was not found.");

            room.Name = dto.Name;
            room.Capacity = dto.Capacity;
            room.BasePricePerHour = dto.BasePricePerHour;

            // Remove services that are in the DB but NOT in the incoming DTO
            var incomingServiceIds = dto.Services
                .Where(s => s.Id.HasValue)
                .Select(s => s.Id!.Value)
                .ToList();

            var servicesToRemove = room.Services
                .Where(s => !incomingServiceIds.Contains(s.Id))
                .ToList();

            foreach (var service in servicesToRemove)
            {
                room.Services.Remove(service);
            }

            // Update existing services and Add new ones
            foreach (var serviceDto in dto.Services)
            {
                if (serviceDto.Id.HasValue)
                {
                    var existingService = room.Services.FirstOrDefault(s => s.Id == serviceDto.Id.Value);
                    if (existingService != null)
                    {
                        existingService.Name = serviceDto.Name;
                        existingService.Price = serviceDto.Price;
                    }
                }
                else
                {
                    room.Services.Add(new RoomService
                    {
                        Name = serviceDto.Name,
                        Price = serviceDto.Price
                    });
                }
            }

            await _roomRepository.UpdateAsync(room, cancellationToken);
        }

        public async Task DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {id} was not found.");

            await _roomRepository.DeleteAsync(room, cancellationToken);
        }

        public async Task<IEnumerable<RoomDto>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity, CancellationToken cancellationToken = default)
        {
            if (startTime == default || endTime == default)
                throw new ArgumentException("Both 'start' and 'end' dates must be provided.");

            if (startTime >= endTime)
                throw new ArgumentException("'start' date must be earlier than 'end' date.");

            if (capacity <= 0)
                throw new ArgumentException("'capacity' must be greater than 0.");

            var rooms = await _roomRepository.GetAvailableRoomsAsync(startTime, endTime, capacity, cancellationToken);

            return rooms.Select(r => new RoomDto(
                r.Id,
                r.Name,
                r.Capacity,
                r.BasePricePerHour,
                r.Services.Select(s => new RoomServiceDto(s.Id, s.Name, s.Price)).ToList()
            ));
        }
    }
}
