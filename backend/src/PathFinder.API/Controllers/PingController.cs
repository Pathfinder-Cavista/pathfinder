using Microsoft.AspNetCore.Mvc;

namespace PathFinder.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
