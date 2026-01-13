
namespace School_IServices
{
    public interface IAIContentService
    {
        Task<string> GenerateContentAsync(string prompt);
    }
}
