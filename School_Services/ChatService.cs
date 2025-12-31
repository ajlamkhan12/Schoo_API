using AutoMapper;
using Data.Data;
using DbModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using School_IServices;
using School_View_Models;

namespace School_Services
{


    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly School_ManagementContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ICommunicationService _communicationService;
        public ChatService(IHubContext<ChatHub> hub, School_ManagementContext context, IMapper mapper, IConfiguration configuration, ICommunicationService communicationService)
        {
            _hub = hub;
            _context = context;
            _mapper = mapper;
            _config = configuration;
            _communicationService = communicationService;
        }

        public async Task SendPrivateMessage(ChatViewModel model)
        {
            // 🔹 Save to DB here if needed
            await _communicationService.AddChat(model);
            // 🔹 Send to specific user
            if (model.RecieverId.HasValue)
            {
                await _hub.Clients.User(model.RecieverId.Value.ToString())
                    .SendAsync("ReceiveMessage", model);
            }

            // 🔹 Also send back to sender
            await _hub.Clients.User(model.SenderId.ToString())
                .SendAsync("ReceiveMessage", model);
        }

        public async Task SendGroupMessage(string groupId, ChatViewModel model)
        {
            await _hub.Clients.Group(groupId)
                .SendAsync("ReceiveMessage", model);
        }
    }

}
