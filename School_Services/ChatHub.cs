using Microsoft.AspNetCore.SignalR;
using School_IServices;
using School_View_Models;
using System.Collections.Concurrent;

namespace School_Services
{
    public class ChatHub : Hub
    {
        private readonly ICommunicationService _communicationService;

        public ChatHub(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            Console.WriteLine($"User connected: {userId}, ConnId: {Context.ConnectionId}");

            if (!string.IsNullOrEmpty(userId))
            {
                UserConnectionManager.Users[userId] = Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                UserConnectionManager.Users.TryRemove(userId, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // ================= 1-TO-1 CHAT =================
        public async Task SendPrivateMessage(string toUserId, string content)
        {
            var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            Console.WriteLine($"Sender: {senderUserId}, Receiver: {toUserId}");

            if (string.IsNullOrEmpty(senderUserId))
                return;

            var message = new ChatViewModel
            {
                SenderId = int.Parse(senderUserId),
                RecieverId = int.Parse(toUserId),
                Content = content,
                MessageType = 1,
                IsGroup = false,
                CreatedOn = DateTime.UtcNow,
                SenderName = _communicationService.GetSenderName(int.Parse(senderUserId))
            };

            // SAVE TO DB
            message = await _communicationService.AddChat(message);

            // SEND TO SENDER
            await Clients.Caller.SendAsync("ReceiveMessage", message);

            // SEND TO RECEIVER
            if (UserConnectionManager.Users.TryGetValue(toUserId, out var connId))
            {
                Console.WriteLine($"Receiver online: {connId}");
                await Clients.Client(connId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                Console.WriteLine("Receiver NOT online");
            }
        }

        // ================= GROUP CHAT =================
        public async Task SendGroupMessage(int groupId, string content)
        {
            var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (string.IsNullOrEmpty(senderUserId)) return;

            var senderId = int.Parse(senderUserId);

            var message = new ChatViewModel
            {
                SenderId = senderId,
                GroupId = groupId,
                Content = content,
                MessageType = 2,
                IsGroup = true,
                CreatedOn = DateTime.UtcNow,
                SenderName = _communicationService.GetSenderName(int.Parse(senderUserId))
            };

            message = await _communicationService.AddChat(message);

            await Clients.Caller.SendAsync("ReceiveMessage", message);

            var members = await _communicationService.GetGroupMembers(groupId);

            foreach (var member in members)
            {
                if (member == senderId) continue;

                if (UserConnectionManager.Users.TryGetValue(member.ToString(), out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveMessage", message);
                }
            }
        }
    }

    public static class UserConnectionManager
    {
        public static ConcurrentDictionary<string, string> Users
            = new ConcurrentDictionary<string, string>();
    }
}
