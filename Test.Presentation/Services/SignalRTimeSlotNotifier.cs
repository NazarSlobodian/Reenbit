using Microsoft.AspNetCore.SignalR;
using Test.Application.Interfaces.Services;
using Test.Domain.Entities;
using Test.Presentation.Hubs;
using Test.Presentation.Hubs.Clients;

namespace Test.Presentation.Services
{
    public class SignalRTimeSlotNotifier : ITimeSlotNotifier
    {
        private readonly IHubContext<BookingHub, IBookingClient> _hubContext;

        public SignalRTimeSlotNotifier(IHubContext<BookingHub, IBookingClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTimeSlotStatusChangedAsync(Guid roomId, Guid timeSlotId, TimeSlotStatus newStatus, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.Group(roomId.ToString())
                .TimeSlotStatusChanged(timeSlotId, (int)newStatus);
        }
    }
}