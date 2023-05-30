using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Runtime.InteropServices;

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

    /// <summary>
    /// Create new location
    /// </summary>
    /// <param name="dto">CreateLocationDto</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("CreateLocation")]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto dto, [Optional][FromQuery] bool force)
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

        await _locationPlatform.CreateLocationAsync(dto.ToEntity(force));
        return Ok();
    }

    /// <summary>
    /// Get all locations
    /// </summary>
    /// <param name="limit">Limit of location present in return</param>
    [ProducesResponseType(typeof(List<LocationDto>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<LocationDto>>> GetLocationsAsync([Optional][FromQuery] int limit)
    {
        IEnumerable<Location> locations = await _locationPlatform.GetAllLocationAsync(limit);
        return locations.ToList_LocationDto();
    }

    /// <summary>
    /// Get location by id
    /// </summary>
    /// <param name="locationId">Id of wanted location</param>
    [ProducesResponseType(typeof(GetLocationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return location.ToGetLocationDto();
    }

    /// <summary>
    /// Get location by name
    /// </summary>
    /// <param name="locationName">Name of wanted Location</param>
    [ProducesResponseType(typeof(GetLocationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return location.ToGetLocationDto();
    }

    /// <summary>
    /// Get all location by game id
    /// </summary>
    /// <param name="gameId">Id of related game</param>
    [ProducesResponseType(typeof(List<LocationDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return locations.ToList_LocationDto();
    }

    /// <summary>
    /// Get all location by game name
    /// </summary>
    /// <param name="gameName">Name of related game</param>
    [ProducesResponseType(typeof(List<LocationDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return locations.ToList_LocationDto();
    }

    /// <summary>
    /// Make a request to update a location
    /// </summary>
    /// <param name="locationId">Id of location to request an update</param>
    /// <param name="dto">LocationUpdateRequestDto</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("{locationId}")]
    public async Task<ActionResult<MessageDto>> CreateUpdateRequest([FromRoute] Guid locationId, [FromBody] LocationUpdateRequestDto dto)
    {

        if (locationId != dto.LocationId)
            return BadRequest(new MessageDto(LocationMessage.IdWithQueryAndDtoAreDifferent));

        Location? entity = await _locationPlatform.GetLocationByIdAsync(dto.LocationId);
        if (entity is null)
            return BadRequest(new MessageDto(LocationMessage.NotFoundById));

        await _locationPlatform.CreateUpdateRequestAsync(dto.ToEntity());
        return new MessageDto(LocationMessage.LocationUpdateRequestSuccess);
    }

    /// <summary>
    /// Get location with all request update
    /// </summary>
    /// <param name="locationId">Id of location</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ListLocationUpdateRequest), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(NotFound), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    [Route("Request_Update/{locationId}")]
    public async Task<ActionResult<ListLocationUpdateRequest>> GetLocationWithAllRequestUpdate([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationWithRequestUpdateAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        if (location.RequestLocationUpdates is null || !location.RequestLocationUpdates.Any())
            return BadRequest(new MessageDto(LocationMessage.NotFoundUpdateRequest));

        return location.ToListLocationUpdateRequest();
    }

    /// <summary>
    /// Update location -> For Admin only
    /// </summary>
    /// <param name="locationId">Id of location to update</param>
    /// <param name="dto">UpdateLocationDto</param>
    /// <param name="requestUpdateId">If used, this means that the update is performed following validation of a request</param>
    [ProducesResponseType(typeof(GetLocationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    [Route("{locationId}")]
    public async Task<ActionResult<GetLocationDto>> UpdateLocation([FromRoute] Guid locationId, [FromBody] UpdateLocationDto dto, [Optional][FromQuery] Guid? requestUpdateId)
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

        if (requestUpdateId is not null)
            await _locationPlatform.DeleteUpdateRequestAsync(requestUpdateId);

        return location.ToGetLocationDto();
    }

    /// <summary>
    /// Delete location by id
    /// </summary>
    /// <param name="locationId">Id of deleted location</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("Delete/{locationId}")]
    public async Task<IActionResult> DeleteLocationByIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        }

        await _locationPlatform.DeleteLocationAsync(location);

        return Ok(new MessageDto(LocationMessage.SuccesDeleted));
    }

    /// <summary>
    /// Request Update Game by Id
    /// </summary>
    /// <param name="requestUpdateId">Id of request UpdateId Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("DeleteRequestUpdate/{requestUpdateId}")]
    public async Task<ActionResult<MessageDto>> DeleteRequestUpdateLocationById([FromRoute] Guid requestUpdateId)
    {
        RequestLocationUpdate? requestLocationUpdate = await _locationPlatform.GetRequestUpdateLocationByIdAsync(requestUpdateId);
        if (requestLocationUpdate is null)
        {
            return NotFound(new MessageDto(LocationMessage.RequestUpdateNotFoundById));
        }

        await _locationPlatform.DeleteRequestLocationUpdateAsync(requestLocationUpdate);

        return new MessageDto(LocationMessage.RequestUpdateSuccesDeleted);
    }
}