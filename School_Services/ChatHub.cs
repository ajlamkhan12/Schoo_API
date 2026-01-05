using Microsoft.AspNetCore.SignalR;
using School_View_Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace School_Services
{
    //public class ChatHub : Hub
    //{
    //    public async Task SendMessage(ChatViewModel message)
    //    {
    //        await Clients.All.SendAsync("ReceiveMessage", message);
    //    }

    //    public async Task JoinGroup(string groupId)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    //    }

    //    public async Task SendGroupMessage(string groupId, ChatViewModel message)
    //    {
    //        await Clients.Group(groupId).SendAsync("ReceiveMessage", message);
    //    }
    //}



    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
                UserConnectionManager.Users[userId] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        //    if (!string.IsNullOrEmpty(userId))
        //        UserConnectionManager.Users.Remove(userId);

        //    return base.OnDisconnectedAsync(exception);
        //}


        // 1-to-1 Message
        public async Task SendPrivateMessage(int toUserId, string content)
        {
            try
            {
                var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
                if (string.IsNullOrEmpty(senderUserId)) throw new Exception("Sender userId is missing");

                if (!UserConnectionManager.Users.TryGetValue(toUserId.ToString(), out var connId))
                    throw new Exception("Recipient not connected");

                var message = new ChatViewModel
                {
                    Id = 0,
                    SenderId = int.Parse(senderUserId),
                    RecieverId = toUserId,
                    MessageType = 1,
                    Content = content,
                    IsViewed = false,
                    CreatedOn = DateTime.UtcNow,
                    SenderName = "Ajlam Khan"
                };

                await Clients.Client(connId)
                             .SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendPrivateMessage Error: " + ex.Message);
                throw;
            }
        }



        // Create Group
        public async Task CreateGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName)
                .SendAsync("GroupCreated", groupName);
        }

        // Join Group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName)
                .SendAsync("UserJoined", groupName);
        }

        // Leave Group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName)
                .SendAsync("UserLeft", groupName);
        }

        // Group Message
        public async Task SendGroupMessage(int groupId, string content)
        {
            var senderUserId = Context.GetHttpContext()
                                       ?.Request.Query["userId"]
                                       .ToString();

            // Optional: check if the sender is part of the group
            // You can implement your own GroupManager to track group memberships

            var message = new ChatViewModel
            {
                Id = 0, // DB generated later
                SenderId = int.Parse(senderUserId),
                GroupId = groupId,
                MessageType = 2, // assuming 2 means group message
                Content = content,
                IsViewed = false,
                CreatedOn = DateTime.UtcNow,
                SenderName = "Ajlam Khan" // fetch from DB if needed
            };

            // Send to all clients in the group
            await Clients.Group(groupId.ToString())
                         .SendAsync("ReceiveMessage", message);
        }


    }
    public static class UserConnectionManager
    {
        // userId -> connectionId
        public static ConcurrentDictionary<string, string> Users
            = new ConcurrentDictionary<string, string>();
    }
}
