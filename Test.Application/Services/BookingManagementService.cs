using Test.Application.DTOs.Bookings;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;

using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class BookingManagementService : IBookingManagementService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingManagementService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.StartTime == default || dto.EndTime == default)
                throw new ArgumentException("Start and End times must be provided.");

            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("Start time must be before End time.");

            if (dto.StartTime < DateTime.UtcNow)
                throw new ArgumentException("Cannot book a room in the past.");

            var room = await _roomRepository.GetByIdAsync(dto.RoomId, cancellationToken);
            if (room == null) throw new KeyNotFoundException($"Room with ID {dto.RoomId} was not found.");

            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(dto.StartTime, dto.EndTime, 1, cancellationToken);
            if (!availableRooms.Any(r => r.Id == dto.RoomId))
                throw new InvalidOperationException("Room is already booked for the selected time.");

            // Snapshotting the services and prices at the time of booking.
            // This ensures historical financial data (Analytics) remains accurate 
            // even if an admin changes the base price of a service in the future.
            var bookingServices = room.Services
                .Where(s => dto.SelectedServiceIds.Contains(s.Id))
                .Select(s => new BookingService
                {
                    RoomServiceId = s.Id,
                    ServiceName = s.Name,
                    PriceAtBooking = s.Price
                }).ToList();

            decimal servicesTotal = bookingServices.Sum(s => s.PriceAtBooking);
            decimal roomTotal = CalculateRoomPrice(room.BasePricePerHour, dto.StartTime, dto.EndTime);

            room.UpdatedAt = DateTime.UtcNow;

            var booking = new Booking
            {
                RoomId = room.Id,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                RoomPriceTotal = roomTotal,
                ServicesPriceTotal = servicesTotal,
                TotalPrice = roomTotal + servicesTotal,
                BookingServices = bookingServices
            };

            await _bookingRepository.AddAsync(booking, cancellationToken);

            return new BookingResponseDto(booking.Id, booking.RoomPriceTotal, booking.ServicesPriceTotal, booking.TotalPrice);
        }

        // Calculating price minute-by-minute to correctly handle bookings that cross 
        // multiple tariff periods (e.g., booking from 11:30 to 13:30 crosses standard and peak hours).
        private decimal CalculateRoomPrice(decimal basePricePerHour, DateTime start, DateTime end)
        {
            decimal total = 0;
            var current = start;

            while (current < end)
            {
                var multiplier = GetMultiplier(current.TimeOfDay);
                total += (basePricePerHour * multiplier) / 60m;
                current = current.AddMinutes(1);
            }

            return Math.Round(total, 2);
        }

        private decimal GetMultiplier(TimeSpan time)
        {
            if (time >= new TimeSpan(6, 0, 0) && time < new TimeSpan(9, 0, 0)) return 0.9m;
            if (time >= new TimeSpan(12, 0, 0) && time < new TimeSpan(14, 0, 0)) return 1.15m;
            if (time >= new TimeSpan(9, 0, 0) && time < new TimeSpan(18, 0, 0)) return 1.0m;
            if (time >= new TimeSpan(18, 0, 0) && time < new TimeSpan(23, 0, 0)) return 0.8m;
            return 1.0m;
        }
    }
}