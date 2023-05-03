using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Errors;
using GameTrip.Domain.Extension;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
#if !DEBUG
[Authorize]
#endif
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationPlarform _locationPlarform;
    private readonly IValidator<LocationDto> _locationValidator;

    public LocationController(ILocationPlarform locationPlarform, IValidator<LocationDto> locationValidator)
    {
        _locationPlarform = locationPlarform;
        _locationValidator = locationValidator;
    }

    [HttpPost]
    [Route("CreateLocation")]
    public IActionResult CreateLocation(LocationDto dto)
    {
        ValidationResult result = _locationValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return BadRequest(ModelState);
        }

        Location? location = _locationPlarform.GetLocationByName(dto.Name);
        if (location is not null)
            return BadRequest(new ErrorResultDTO(LocationErrors.AlreadyExistByName));

        location ??= _locationPlarform.GetLocationByPosition(dto.Latitude, dto.Longitude);
        if (location is not null)
            return BadRequest(new ErrorResultDTO(LocationErrors.AlreadyExistByPos));

        _locationPlarform.CreateLocation(dto.ToEntity());
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Locations")]
    public async Task<ActionResult<List<LocationDto>>> LocationsAsync()
    {
        IEnumerable<Location> locations = await _locationPlarform.GetAllLocationAsync();
        return locations.ToDtoList();
    }
}