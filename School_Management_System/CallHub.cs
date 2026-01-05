using Microsoft.AspNetCore.SignalR;
using School_Services;

namespace School_Management_System
{
    public class CallHub : Hub
    {
        public async Task CallUser(string userId)
        {
            var connId = UserConnectionManager.Users[userId];
            await Clients.Client(connId).SendAsync("IncomingCall");
        }

        public async Task SendOffer(string userId, object offer)
        {
            await Clients.Client(UserConnectionManager.Users[userId])
                .SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string userId, object answer)
        {
            await Clients.Client(UserConnectionManager.Users[userId])
                .SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string userId, object candidate)
        {
            await Clients.Client(UserConnectionManager.Users[userId])
                .SendAsync("ReceiveIceCandidate", candidate);
        }
    }
}
