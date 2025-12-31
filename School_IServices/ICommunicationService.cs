using School_View_Models;


namespace School_IServices
{
    public interface ICommunicationService
    {
        Task<List<ContactViewModel>> GetContacts(int userId);
        Task<List<ChatViewModel>> GetOneOnOneChat(int senderId, int receiverId);
        Task<List<ChatViewModel>> GetGroupChat(int groupId);
        Task<List<ChatViewModel>> AddChat(ChatViewModel model);
        Task<bool> AddGroup(GroupViewModel groupViewModel);
    }
}
