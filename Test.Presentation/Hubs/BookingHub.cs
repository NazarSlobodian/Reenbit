using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Test.Presentation.Hubs.Clients;

namespace Test.Presentation.Hubs
{
    [Authorize]
    public class BookingHub : Hub<IBookingClient>
    {
        public async Task JoinRoomGroup(Guid roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public async Task LeaveRoomGroup(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }
}