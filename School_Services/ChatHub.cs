using Microsoft.AspNetCore.SignalR;

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
            return base.OnConnectedAsync();
        }
    
}

}
