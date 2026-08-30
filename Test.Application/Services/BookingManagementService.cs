using Test.Application.DTOs.Bookings;
using Test.Application.Exceptions;
using Test.Application.Interfaces;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;
using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class BookingManagementService : IBookingManagementService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingManagementService(
            IBookingRepository bookingRepository,
            ITimeSlotRepository timeSlotRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _timeSlotRepository = timeSlotRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingDto> CreateBookingAsync(Guid timeSlotId, string userId, CancellationToken cancellationToken = default)
        {
            var timeSlot = await _timeSlotRepository.GetByIdAsync(timeSlotId, cancellationToken);
            if (timeSlot == null)
                throw new KeyNotFoundException($"Time slot {timeSlotId} was not found.");

            if (timeSlot.Status != TimeSlotStatus.Available)
                throw new BookingConflictException("This time slot is no longer available.");

            timeSlot.Status = TimeSlotStatus.Booked;

            var booking = new Booking { TimeSlotId = timeSlot.Id, UserId = userId };
            await _bookingRepository.AddAsync(booking, cancellationToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                throw new BookingConflictException("This time slot was just booked by another user.");
            }

            // TODO: broadcast the time slot status change via SignalR once the notifier is wired up.

            return new BookingDto(booking.Id, timeSlot.Id, timeSlot.RoomId, timeSlot.StartTime, timeSlot.EndTime, booking.UserId);
        }

        public async Task<IEnumerable<BookingDto>> GetMyBookingsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetByUserAsync(userId, cancellationToken);
            return bookings.Select(ToDto);
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
            return bookings.Select(ToDto);
        }

        private static BookingDto ToDto(Booking b) =>
            new(b.Id, b.TimeSlotId, b.TimeSlot.RoomId, b.TimeSlot.StartTime, b.TimeSlot.EndTime, b.UserId);
    }
}