using DbModels;
using Microsoft.AspNetCore.Mvc;
using School_IServices;
using School_View_Models;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;
        public CommunicationController(ICommunicationService communicationService)
        {
           _communicationService = communicationService;
        }

        [HttpGet("contacts/{userId}")]
        public async Task<IActionResult> GetContacts(int userId)
        {
            try
            {
                return Ok(await _communicationService.GetContacts(userId));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("one_one/{senderId}/{reciverId}")]
        public async Task<IActionResult> GetOneOnOneChat(int senderId, int reciverId)
        {
            try
            {
                return Ok(await _communicationService.GetOneOnOneChat(senderId, reciverId));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroupChat(int groupId)
        {
            try
            {
                return Ok(await _communicationService.GetGroupChat(groupId));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddChat([FromBody] ChatViewModel model)
        {
            try
            {
                return Ok(await _communicationService.AddChat(model));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("group")]
        public async Task<IActionResult> AddGroup([FromBody] GroupViewModel model)
        {
            try
            {
              if(await _communicationService.AddGroup(model))
                {
                    return Ok(await _communicationService.GetContacts(model.Admin));
                }
                
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
