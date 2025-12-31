using School_View_Models;
namespace School_IServices
{
    public interface IChatService
    {
        Task SendPrivateMessage(ChatViewModel model);
        Task SendGroupMessage(string groupId, ChatViewModel model);
    }
}
