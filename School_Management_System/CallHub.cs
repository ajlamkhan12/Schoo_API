using Microsoft.AspNetCore.SignalR;
using School_Services;

namespace School_Management_System
{
    public class CallHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
                UserConnectionManager.Users[userId] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Get userId from query
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                // Safely remove the user from dictionary
                UserConnectionManager.Users.TryRemove(userId, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }



        public async Task CallUser(string userId)
        {
            if (UserConnectionManager.Users.TryGetValue(userId, out var connId))
                await Clients.Client(connId).SendAsync("IncomingCall");
        }

        public async Task SendOffer(string userId, object offer)
        {
            if (UserConnectionManager.Users.TryGetValue(userId, out var connId))
                await Clients.Client(connId).SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string userId, object answer)
        {
            if (UserConnectionManager.Users.TryGetValue(userId, out var connId))
                await Clients.Client(connId).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string userId, object candidate)
        {
            if (UserConnectionManager.Users.TryGetValue(userId, out var connId))
                await Clients.Client(connId).SendAsync("ReceiveIceCandidate", candidate);
        }
    }
}
