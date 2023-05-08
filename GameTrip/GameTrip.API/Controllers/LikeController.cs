using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LikeModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
    private readonly IValidator<AddLikeLocationDto> _addLikeLocationValidator;
    private readonly ILocationPlarform _locationPlaform;
    private readonly UserManager<GameTripUser> _userManager;
    private readonly ILikePlatform _likePlatform;

    public LikeController(IValidator<AddLikeLocationDto> addLikeLocationValidator, ILocationPlarform locationPlaform, UserManager<GameTripUser> userManager, ILikePlatform likePlatform)
    {
        _addLikeLocationValidator = addLikeLocationValidator;
        _locationPlaform = locationPlaform;
        _userManager = userManager;
        _likePlatform = likePlatform;
    }

    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddLikedLocation")]
    public async Task<ActionResult<LikedLocationDto>> LikeLocation([FromBody] AddLikeLocationDto dto)
    {
        ValidationResult validationResult = _addLikeLocationValidator.Validate(dto);
        if (validationResult.IsValid is false)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Location? location = await _locationPlaform.GetLocationByIdAsync((Guid)dto.LocationId!);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(dto.UserId!.ToString()!);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (location.LikedLocations!.Any(l => l.UserId == dto.UserId))
            return BadRequest(new MessageDto(LikeMessage.UserAlreadyLikeLocation));

        await _likePlatform.AddLikeToLocationAsync(location, user, (decimal)dto.Value);

        IEnumerable<LikedLocation> likedLocations = _likePlatform.GetAllLikedLocationByLocation(location);

        //Create Mapper
        return Ok(likedLocations.ToDto());
    }
}
