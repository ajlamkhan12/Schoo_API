using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_IServices;

namespace School_Management_System.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIContentService _aiService;

        public AIController(IAIContentService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] PromptRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Prompt))
                    return BadRequest("Prompt cannot be empty.");

                var result = await _aiService.GenerateContentAsync(request.Prompt);
                return Ok(new { Response = result });
            }
            catch (Exception ex) 
            {

                throw;
            }
           
        }
    }

    // Request DTO
    public class PromptRequest
    {
        public string Prompt { get; set; }
    }
}
