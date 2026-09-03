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
        private readonly ITimeSlotNotifier _notifier;

        public BookingManagementService(
            IBookingRepository bookingRepository,
            ITimeSlotRepository timeSlotRepository,
            IUnitOfWork unitOfWork,
            ITimeSlotNotifier notifier)
        {
            _bookingRepository = bookingRepository;
            _timeSlotRepository = timeSlotRepository;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
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

            await _notifier.NotifyTimeSlotStatusChangedAsync(timeSlot.RoomId, timeSlot.Id, timeSlot.Status, cancellationToken);

            return new BookingDto(booking.Id, timeSlot.Id, timeSlot.RoomId, timeSlot.StartTime, timeSlot.EndTime, booking.UserId);
        }

        public async Task<IEnumerable<MyBookingDto>> GetMyBookingsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetByUserAsync(userId, cancellationToken);
            return bookings.Select(b => new MyBookingDto(
                b.Id,
                b.TimeSlot.RoomId,
                b.TimeSlot.Room?.Name ?? b.TimeSlot.RoomId.ToString(),
                b.TimeSlot.StartTime,
                b.TimeSlot.EndTime
            ));
        }

        public async Task<IEnumerable<AdminBookingDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
            return bookings.Select(b => new AdminBookingDto(
                b.Id,
                b.TimeSlot.RoomId,
                b.TimeSlot.Room?.Name ?? b.TimeSlot.RoomId.ToString(),
                b.TimeSlot.StartTime,
                b.TimeSlot.EndTime,
                b.UserId
            ));
        }
    }
}