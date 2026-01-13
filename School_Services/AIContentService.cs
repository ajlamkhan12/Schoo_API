using Google.GenAI;
using School_IServices;
namespace School_Services
{
    public class AIContentService : IAIContentService
    {
        private readonly Client _client;

        public AIContentService()
        {
            //_client = new Client(apiKey: "AIzaSyC8rO7waiubV1U8gDl5US2lsEsNbNYQ81E");
            //_client = new Client(apiKey: "AIzaSyD25k-HY2Kmx0PerJQd4pgJKCSbTcpGCqU");
            _client = new Client(apiKey: "AIzaSyCdb0E2kMI1Nj8Hg_JfwNiAo-KzhPKl90o");
        }

        public async Task<string> GenerateContentAsync(string prompt)
        {
            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash",
                contents: prompt
            );

            // Return the first candidate's first part text
            return response.Candidates[0].Content.Parts[0].Text;
        }
    }

}
