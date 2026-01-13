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


        // 1-to-1 Message
        public async Task SendPrivateMessage(string toUserId, string content)
        {
            var senderUserId = Context.GetHttpContext()?.Request.Query["userId"];
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
                SenderName = "Ajlam Khan"
            };

            // 🔹 SAVE TO DB
            message = await _communicationService.AddChat(new ChatViewModel
            {
                SenderId = message.SenderId,
                RecieverId = message.RecieverId,
                Content = message.Content,
                MessageType = 1,
                IsGroup = false,
                CreatedDate = DateTime.UtcNow
            });

            // 🔹 SEND TO SENDER (REAL-TIME)
            await Clients.Caller.SendAsync("ReceiveMessage", message);

            // 🔹 SEND TO RECEIVER (REAL-TIME)
            if (UserConnectionManager.Users.TryGetValue(toUserId.ToString(), out var connId))
            {
                await Clients.Client(connId)
                    .SendAsync("ReceiveMessage", message);
            }
        }




        // Create Group
        public async Task CreateGroup(GroupViewModel model)
        {
            try
            {
                var groupId = await _communicationService.AddGroup(model);
                if (groupId > 0)
                {
                    // 🔹 4. Add ONLINE users to SignalR group
                    foreach (var userId in model.Members.Append(model.Admin))
                    {
                        if (UserConnectionManager.Users
                            .TryGetValue(userId.ToString(), out var connectionId))
                        {
                            await Groups.AddToGroupAsync(connectionId, groupId.ToString());
                        }
                    }

                    // 🔹 5. Notify group members
                    await Clients.Group(groupId.ToString())
                        .SendAsync("GroupCreated", new
                        {
                            GroupId = groupId,
                            model.Title,
                            model.Group_Image_Url,
                            Admin = model.Admin,
                            Members = model.Members
                        });
                }
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
        }

        // Join Group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName)
                .SendAsync("UserJoined", groupName);
        }

        // Leave Group
        public async Task LeaveGroup(int groupId)
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            if (string.IsNullOrEmpty(userId))
                throw new HubException("UserId missing");


            var result = await _communicationService.RemoveMemberFromGroupAsync(groupId, int.Parse(userId.Value));
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                groupId.ToString()
            );

            await Clients.Group(groupId.ToString())
                .SendAsync("UserLeft", new
                {
                    GroupId = groupId,
                    UserId = int.Parse(userId)
                });
        }



        public async Task SendGroupMessage(int groupId, string content)
        {
            var senderUserId = Context.GetHttpContext()?.Request.Query["userId"];
            if (string.IsNullOrEmpty(senderUserId))
                return;

            var senderId = int.Parse(senderUserId);

            // 🔹 CREATE MESSAGE MODEL
            var message = new ChatViewModel
            {
                SenderId = senderId,
                GroupId = groupId,
                Content = content,
                MessageType = 2,
                IsGroup = true,
                IsViewed = false,
                CreatedOn = DateTime.UtcNow,
                SenderName = "Ajlam Khan"
            };

            // 🔹 SAVE TO DB
            message = await _communicationService.AddChat(new ChatViewModel
            {
                SenderId = senderId,
                GroupId = groupId,
                Content = content,
                MessageType = 2,
                IsGroup = true,
                CreatedDate = DateTime.UtcNow
            });

            // 🔹 SEND TO SENDER (IMMEDIATE UI UPDATE)
            await Clients.Caller.SendAsync("ReceiveMessage", message);

            // 🔹 GET ALL GROUP MEMBERS FROM DB
            var groupMembers = await _communicationService.GetGroupMembers(groupId);
            // Expected: List<int> of UserIds

            // 🔹 SEND TO EACH ONLINE MEMBER (EXCEPT SENDER)
            foreach (var memberUserId in groupMembers)
            {
                
                if (memberUserId == senderId) continue;

                if (UserConnectionManager.Users.TryGetValue(memberUserId.ToString(), out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveMessage", message);
                }
            }
        }

    }
    public static class UserConnectionManager
    {
        // userId -> connectionId
        public static ConcurrentDictionary<string, string> Users
            = new ConcurrentDictionary<string, string>();
    }
}
