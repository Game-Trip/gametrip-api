using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.SearchModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[Route("[controller]")]
[Authorize]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchPlatform _searchPlatform;

    public SearchController(ISearchPlatform searchPlatform) => _searchPlatform = searchPlatform;

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("SearchLocation")]
    public async Task<ActionResult<IEnumerable<LocationNameDto>>> SearchLocation([FromQuery] SearchLocationDto dto)
    {
        IEnumerable<Location> locations = await _searchPlatform.SearchLocationAsync(dto);
        return Ok(locations.ToEnumerable_LocationNameDto());
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("SearchGame")]
    public async Task<ActionResult<IEnumerable<GameNameDto>>> SearchGame([FromQuery] SearchGameDto dto)
    {
        IEnumerable<Game> games = await _searchPlatform.SearchGameAsync(dto);
        return Ok(games.ToEnumerable_GameNameDto());
    }
}
