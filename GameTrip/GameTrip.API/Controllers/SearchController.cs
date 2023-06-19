using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.SearchModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchPlatform _searchPlatform;

    public SearchController(ISearchPlatform searchPlatform) => _searchPlatform = searchPlatform;

    [AllowAnonymous]
    [HttpGet]
    [Route("SearchLocation")]
    public async Task<IEnumerable<LocationNameDto>> SearchLocation([FromQuery] SearchLocationDto dto)
    {
        IEnumerable<Location> locations = await _searchPlatform.SearchLocationAsync(dto);
        return locations.ToEnumerable_LocationNameDto();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("SearchGame")]
    public async Task<IEnumerable<SearchedGameDto>> SearchGame([FromQuery] SearchGameDto dto)
    {
        IEnumerable<Game> games = await _searchPlatform.SearchGameAsync(dto);
        return games.ToEnumerable_SearchedGameDto();
    }
}
