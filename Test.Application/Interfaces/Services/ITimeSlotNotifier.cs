using Test.Domain.Entities;

namespace Test.Application.Interfaces.Services
{
    public interface ITimeSlotNotifier
    {
        Task NotifyTimeSlotStatusChangedAsync(Guid roomId, Guid timeSlotId, TimeSlotStatus newStatus, CancellationToken cancellationToken = default);
    }
}