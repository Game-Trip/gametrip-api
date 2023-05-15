using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
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
