using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartupController : ControllerBase
    {

        private readonly IStartupPlatform _startupPlatform;

        public StartupController(IStartupPlatform startupPlatform)
        {
            _startupPlatform = startupPlatform;
        }


        [HttpGet]
        [Route("ping")]
        public ActionResult<string> Ping() => Ok(_startupPlatform.ping());

    }
}
