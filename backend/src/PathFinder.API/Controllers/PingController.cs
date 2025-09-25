using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Filters;

namespace PathFinder.API.Controllers
{
    [Route("api")]
    [ApiController]
    [AnalyticsPermission]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { status = "ok" });
        }
    }
}
