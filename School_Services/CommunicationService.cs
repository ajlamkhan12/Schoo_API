using AutoMapper;
using Data.Data;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using School_IServices;
using School_View_Models;

namespace School_Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly School_ManagementContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public CommunicationService(School_ManagementContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<List<ContactViewModel>> GetContacts(int userId)
        {
            try
            {
                var result = new List<ContactViewModel>();

                var users = await _context.Users
                    .Where(u => u.Id != userId)
                    .ToListAsync();

                foreach (var user in users)
                {
                    result.Add(new ContactViewModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        LastMessage = "Test", /*await GetLastMessage(userId, user.Id)*/
                        IsGroup = false
                    });
                }

                // 2️⃣ Groups (group chat)
                var groups = await _context.GroupMembers
                    .Where(gm => gm.MemberId == userId && !gm.IsDeleted)
                    .Include(gm => gm.Group)
                    .ToListAsync();

                foreach (var gm in groups)
                {
                    if (gm.Group == null) continue;

                    result.Add(new ContactViewModel
                    {
                        Id = gm.Group.Id,
                        Name = gm.Group.Title,
                        LastMessage = "Test", /* await GetLastGroupMessage(gm.Group.Id)*/
                        IsGroup = true
                    });
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ChatViewModel>> AddChat(ChatViewModel model)
        {
            var chat = _mapper.Map<Chat>(model);
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
            if (model.GroupId > 0)
            {
                return await GetGroupChat(model.GroupId.Value);
            }
            else
            {
                return await GetOneOnOneChat(model.SenderId, model.RecieverId.Value);
            }
        }
        public async Task<List<ChatViewModel>> GetOneOnOneChat(int senderId, int receiverId)
        {
            var chats = await _context.Chats
                .Where(c =>
                    (c.SenderId == senderId && c.RecieverId == receiverId) ||
                    (c.SenderId == receiverId && c.RecieverId == senderId))
               .ToListAsync();
            if (chats.Any())
            {
                return (_mapper.Map<List<ChatViewModel>>(chats));
            }
            return new List<ChatViewModel>();

        }

        public async Task<List<ChatViewModel>> GetGroupChat(int groupId)
        {
            var groupChats = await _context.Chats
                .Where(c => c.GroupId == groupId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
            if (groupChats.Any())
            {
                return (_mapper.Map<List<ChatViewModel>>(groupChats));
            }
            return new List<ChatViewModel>();
        }


        private async Task<string> GetLastMessage(int senderId, int receiverId)
        {
            return await _context.Chats
                .Where(c =>
                    (c.SenderId == senderId && c.RecieverId == receiverId) ||
                    (c.SenderId == receiverId && c.RecieverId == senderId))
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => c.Content)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        private async Task<string> GetLastGroupMessage(int groupId)
        {
            return await _context.Chats
                .Where(c => c.GroupId == groupId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => c.Content)
                .FirstOrDefaultAsync() ?? string.Empty;
        }



    }
}
