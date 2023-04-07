using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class StartupController : ControllerBase
    {

        private readonly IStartupPlatform _startupPlatform;
        private readonly IUnitOfWork _unitOfWork;

        public StartupController(IStartupPlatform startupPlatform, IUnitOfWork unitOfWork)
        {
            _startupPlatform = startupPlatform;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("ping")]
        public ActionResult<string> Ping() => Ok(_startupPlatform.ping());

    }
}
