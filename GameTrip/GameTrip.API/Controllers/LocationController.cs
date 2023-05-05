using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationPlarform _locationPlatform;
    private readonly IGamePlatform _gamePlatform;
    private readonly IValidator<CreateLocationDto> _createLocationValidator;
    private readonly IValidator<UpdateLocationDto> _updateLocationValidator;

    public LocationController(ILocationPlarform locationPlarform, IValidator<CreateLocationDto> locationValidator, IGamePlatform gamePlatform, IValidator<UpdateLocationDto> updateLocationValidator)
    {
        _locationPlatform = locationPlarform;
        _createLocationValidator = locationValidator;
        _gamePlatform = gamePlatform;
        _updateLocationValidator = updateLocationValidator;
    }

    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("CreateLocation")]
    public async Task<IActionResult> CreateLocation(CreateLocationDto dto)
    {
        ValidationResult result = _createLocationValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return BadRequest(ModelState);
        }

        Location? location = await _locationPlatform.GetLocationByNameAsync(dto.Name);
        if (location is not null)
            return BadRequest(new MessageDto(LocationMessage.AlreadyExistByName));

        location ??= await _locationPlatform.GetLocationByPositionAsync(dto.Latitude, dto.Longitude);
        if (location is not null)
            return BadRequest(new MessageDto(LocationMessage.AlreadyExistByPos));

        await _locationPlatform.CreateLocationAsync(dto.ToEntity());
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<LocationDto>>> GetLocationsAsync()
    {
        IEnumerable<Location> locations = await _locationPlatform.GetAllLocationAsync();
        return locations.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Id/{locationId}")]
    public async Task<ActionResult<GetLocationDto>> GetLocationByIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        }

        return location.ToDto();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Name/{locationName}")]
    public async Task<ActionResult<GetLocationDto>> GetLocationByNameAsync([FromRoute] string locationName)
    {
        Location? location = await _locationPlatform.GetLocationByNameAsync(locationName);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundByName));
        }

        return location.ToDto();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Game/Id/{gameId}")]
    public async Task<ActionResult<List<LocationDto>>> GetLocationByGameId([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        IEnumerable<Location?> locations = await _locationPlatform.GetLocationByGameIdAsync(game.IdGame);
        if (!locations.Any())
            return NotFound(new MessageDto(LocationMessage.NotFoundByGameId));

        return locations.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Game/Name/{gameName}")]
    public async Task<ActionResult<List<LocationDto>>> GetLocationByGameId([FromRoute] string gameName)
    {
        Game? game = await _gamePlatform.GetGameByNameAsync(gameName);
        if (game is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundByName));

        IEnumerable<Location?> locations = await _locationPlatform.GetLocationByGameNameAsync(gameName);
        if (!locations.Any())
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundByGameName));
        }

        return locations.ToDtoList();
    }

    [Authorize(Roles = Roles.User)]
    [HttpPut]
    [Route("{locationId}")]
    public async Task<ActionResult<GameDto>> UpdateLocation([FromRoute] Guid locationId, [FromBody] UpdateLocationDto dto)
    {
        ValidationResult result = _updateLocationValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        if (locationId != dto.LocationId)
            return BadRequest(new MessageDto(LocationMessage.IdWithQueryAndDtoAreDifferent));

        Location? entity = await _locationPlatform.GetLocationByIdAsync(dto.LocationId);
        if (entity is null)
            return BadRequest(new MessageDto(LocationMessage.NotFoundById));

        Location location = await _locationPlatform.UpdateLocationAsync(entity, dto);
        return Ok(location.ToDto());
    }

    [HttpDelete]
    [Authorize(Roles = Roles.User)]
    [Route("Delete/{locationId}")]
    public async Task<ActionResult<Location>> DeleteLocationByIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        }

        await _locationPlatform.DeleteLocationAsync(location);

        return Ok(new MessageDto(LocationMessage.SuccesDeleted));
    }
}