using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[ApiController]

public class StatusController : ControllerBase
{
    public StatusController()
    {

    }

    /// <summary>
    /// Ping API
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> PingApi() => Ok();
}
