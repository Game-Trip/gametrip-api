using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.PictureModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PictureController : ControllerBase
{
    private readonly IValidator<AddPictureToLocationDto> _addPictureToLocationValidator;
    private readonly IPicturePlatfrom _picturePlatfrom;
    private readonly ILocationPlarform _locationPlatfrom;
    private readonly IGamePlatform _gamePlatform;

    public PictureController(IValidator<AddPictureToLocationDto> addPictureToLocationValidator, ILocationPlarform locationPlatfrom, IGamePlatform gamePlatform, IPicturePlatfrom picturePlatfrom)
    {
        _addPictureToLocationValidator = addPictureToLocationValidator;
        _locationPlatfrom = locationPlatfrom;
        _gamePlatform = gamePlatform;
        _picturePlatfrom = picturePlatfrom;
    }

    //TODO IN PROD : Changer toute cette merde par un seul DTO avec un Array de byte pour la picture data
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddPictureToLocation/{locationId}")]
    public async Task<IActionResult> AddPictureToLocation(IFormFile pictureData, [FromRoute] Guid locationId, [FromQuery] string name, string description)
    {
        AddPictureToLocationDto dto = new()
        {
            LocationId = locationId,
            Name = name,
            Description = description
        };
        ValidationResult validationResult = _addPictureToLocationValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Location? location = await _locationPlatfrom.GetLocationByIdAsync((Guid)dto.LocationId!);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        await _picturePlatfrom.AddPictureToLocationAsync(pictureData, dto, location);
        return Ok();
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("GetPicturesByLocationId/{locationId}")]
    public async Task<ActionResult<IEnumerable<ListPictureLocationDto>>> GetPicturesByLocationId([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatfrom.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        IEnumerable<Picture> pictures = await _picturePlatfrom.GetPicturesByLocationIdAsync(location);

        List<FileContentResult> fileResults = new();
        foreach (Picture picture in pictures)
        {
            FileContentResult fileResult = new(picture.Data, "image/jpeg");
            fileResults.Add(fileResult);
        }

        return Ok(pictures.ToListPictureLocationDto());
        //return new JsonResult(fileResults);
        //return File(pictures.Last().Data, "image/jpeg");
    }
}
