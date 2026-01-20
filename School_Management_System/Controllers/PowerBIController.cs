using Microsoft.AspNetCore.Mvc;
using School_Services.PowerBIServices;
using School_View_Models.PowerBi_MOdels;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerBIController : Controller
    {
        private readonly PowerBIService _powerBIService;

        public PowerBIController(IConfiguration config)
        {
            var settings = config.GetSection("PowerBI").Get<PowerBISettings>();
            _powerBIService = new PowerBIService(settings);
        }

        //public async Task<IActionResult> Report()
        //{
        //    var embedInfo = await _powerBIService.GetEmbedInfo();
        //    return View(embedInfo);
        //}
    }
}
