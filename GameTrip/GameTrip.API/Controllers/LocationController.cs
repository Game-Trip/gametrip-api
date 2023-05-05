using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
#if !DEBUG
[Authorize(Roles = "User")]
#endif
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationPlarform _locationPlarform;
    private readonly IValidator<CreateLocationDto> _locationValidator;

    public LocationController(ILocationPlarform locationPlarform, IValidator<CreateLocationDto> locationValidator)
    {
        _locationPlarform = locationPlarform;
        _locationValidator = locationValidator;
    }

#if !DEBUG
[Authorize(Roles = "Admin")]
#endif

    [HttpPost]
    [Route("CreateLocation")]
    public async Task<IActionResult> CreateLocation(CreateLocationDto dto)
    {
        ValidationResult result = _locationValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return BadRequest(ModelState);
        }

        Location? location = await _locationPlarform.GetLocationByNameAsync(dto.Name);
        if (location is not null)
            return BadRequest(new MessageDto(LocationMessage.AlreadyExistByName));

        location ??= await _locationPlarform.GetLocationByPositionAsync(dto.Latitude, dto.Longitude);
        if (location is not null)
            return BadRequest(new MessageDto(LocationMessage.AlreadyExistByPos));

        await _locationPlarform.CreateLocationAsync(dto.ToEntity());
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<LocationDto>>> GetLocationsAsync()
    {
        IEnumerable<Location> locations = await _locationPlarform.GetAllLocationAsync();
        return locations.ToDtoList();
    }

    //TODO fix loop DTO
    [AllowAnonymous]
    [HttpGet]
    [Route("Id/{locationId}")]
    public async Task<ActionResult<Location>> GetLocationByIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlarform.GetLocationByIdAsync(locationId);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        }

        return location;
    }

    //TODO fix loop DTO
    [AllowAnonymous]
    [HttpGet]
    [Route("Name/{locationName}")]
    public async Task<ActionResult<Location>> GetLocationByNameAsync([FromRoute] string locationName)
    {
        Location? location = await _locationPlarform.GetLocationByNameAsync(locationName);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundByName));
        }

        return location;
    }

    [HttpDelete]
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
    [Route("Delete/{locationId}")]
    public async Task<ActionResult<Location>> DeleteLocationByIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlarform.GetLocationByIdAsync(locationId);
        if (location is null)
        {
            return NotFound(new MessageDto(LocationMessage.NotFoundById));
        }

        await _locationPlarform.DeleteLocationAsync(location);

        return Ok(new MessageDto(LocationMessage.SuccesDeleted));
    }
}