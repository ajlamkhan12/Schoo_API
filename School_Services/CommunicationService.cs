using AutoMapper;
using Data.Data;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using School_IServices;
using School_View_Models;
using System;


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
                        LastMessage = await GetLastMessage(userId, user.Id),
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
                        LastMessage = await GetLastGroupMessage(gm.Group.Id),
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
            var chat = new Chat
            {
                SenderId = model.SenderId,
                RecieverId = model.RecieverId > 0 ? model.RecieverId :  null, // null or value
                GroupId = model.GroupId > 0 ? model.GroupId : null,
                MessageType = 1,
                Content = model.Content,
                IsViewed = false,
                CreatedBy = model.SenderId
            };

            _context.Chats.Add(chat);
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
                    (c.SenderId == receiverId && c.RecieverId == senderId)).Include(x=>x.Sender)
               .ToListAsync();
            if (chats.Any())
            {
                var messages = chats.Select(c => new ChatViewModel
                {
                    Id = c.Id,
                    SenderId = c.SenderId,
                    RecieverId = c.RecieverId,
                    GroupId = c.GroupId,
                    MessageType = c.MessageType,
                    Reply_To_Message_Id = c.Reply_To_Message_Id,
                    Content = c.Content,
                    IsViewed = c.IsViewed,
                    Media_Url = c.Media_Url,
                    SenderName = c.Sender?.Name,
                    CreatedOn = c.CreatedOn
                }).ToList();
                return messages;
            }
            return new List<ChatViewModel>();

        }

        public async Task<List<ChatViewModel>> GetGroupChat(int groupId)
        {
            var groupChats = await _context.Chats
                .Where(c => c.GroupId == groupId).Include(x=>x.Sender)
                //.OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
            if (groupChats.Any())
            {
                var messages = groupChats.Select(c => new ChatViewModel
                {
                    Id = c.Id,
                    SenderId = c.SenderId,
                    RecieverId = 0,
                    GroupId = c.GroupId,
                    MessageType = c.MessageType,
                    Reply_To_Message_Id = c.Reply_To_Message_Id,
                    Content = c.Content,
                    IsViewed = c.IsViewed,
                    Media_Url = c.Media_Url,
                    SenderName = c.Sender?.Name,
                    CreatedOn = c.CreatedOn
                }).ToList();
                return messages;
            }
            return new List<ChatViewModel>();
        }

        public async Task<bool> AddGroup(GroupViewModel groupViewModel)
        {
            try
            {
                var group = new DbModels.Group
                {
                    Title = groupViewModel.Title,
                    Admin = groupViewModel.Admin.ToString(),
                    CreatedBy = groupViewModel.Admin,
                    Group_Image_Url=groupViewModel.Group_Image_Url
                };
                await _context.Groups.AddAsync(group);  
                await _context.SaveChangesAsync();
                foreach (var member in groupViewModel.Members)
                {
                    var groupMember = new GroupMember
                    {
                        GroupId = group.Id,
                        MemberId = member, // Convert.ToInt32( member),
                        CreatedBy = groupViewModel.Admin,
                        Group_Image_Url = groupViewModel.Group_Image_Url
                    };
                    await _context.GroupMembers.AddAsync(groupMember);
                    await _context.SaveChangesAsync();
                }
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Privtae

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
        #endregion
    }
}
