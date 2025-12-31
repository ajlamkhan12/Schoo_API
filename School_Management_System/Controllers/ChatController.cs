using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using School_IServices;
using School_Services;
using School_View_Models;

namespace School_Management_System.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto message)
        {
            message.CreatedOn = DateTime.Now;

            // Broadcast message to all clients
            await _hubContext.Clients.All
                .SendAsync("ReceiveMessage", message);

            return Ok();
        }

        public class ChatMessageDto
        {
            public int SenderId { get; set; }
            public int RecieverId { get; set; }
            public string SenderName { get; set; }
            public string Content { get; set; }
            public DateTime CreatedOn { get; set; }
        }

    }
 }
