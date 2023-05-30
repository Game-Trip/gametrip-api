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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace GameTrip.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PictureController : ControllerBase
{
    private readonly IValidator<AddPictureToLocationDto> _addPictureToLocationValidator;
    private readonly IValidator<AddPictureToGameDto> _addPictureToGameValidator;
    private readonly IPicturePlatfrom _picturePlatfrom;
    private readonly ILocationPlarform _locationPlatfrom;
    private readonly IGamePlatform _gamePlatform;

    public PictureController(IValidator<AddPictureToLocationDto> addPictureToLocationValidator, ILocationPlarform locationPlatfrom, IGamePlatform gamePlatform, IPicturePlatfrom picturePlatfrom, IValidator<AddPictureToGameDto> addPictureToGameValidator)
    {
        _addPictureToLocationValidator = addPictureToLocationValidator;
        _locationPlatfrom = locationPlatfrom;
        _gamePlatform = gamePlatform;
        _picturePlatfrom = picturePlatfrom;
        _addPictureToGameValidator = addPictureToGameValidator;
    }

    //TODO IN PROD : Changer toute cette merde par un seul DTO avec un Array de byte pour la picture data
    #region Picture and Location
    /// <summary>
    /// Create and add picture to location
    /// </summary>
    /// <param name="pictureData">Picture file</param>
    /// <param name="locationId">Id of location to add picture</param>
    /// <param name="name">Picture name</param>
    /// <param name="description">Picture description</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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
        return Ok(new MessageDto(PictureMessage.SucessAddToLocation));
    }

    /// <summary>
    /// Get all pictures of location
    /// </summary>
    /// <param name="locationId">Id of location</param>
    [ProducesResponseType(typeof(IEnumerable<ListPictureDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("GetPicturesByLocationId/{locationId}")]
    public async Task<ActionResult<IEnumerable<ListPictureDto>>> GetPicturesByLocationId([FromRoute] Guid locationId)
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

        return Ok(pictures.ToEnumerable_ListPictureDto());
    }

    /// <summary>
    /// Get picture by id
    /// </summary>
    /// <param name="pictureId">Id of deleted Picture</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpDelete]
    [Route("DeletePicture/{pictureId}")]
    public async Task<IActionResult> DeletePictureById([FromRoute] Guid pictureId)
    {
        Picture? picture = await _picturePlatfrom.GetPictureByIdAsync(pictureId);
        if (picture is null)
            return NotFound(new MessageDto(PictureMessage.NotFoundById));

        await _picturePlatfrom.DeletePictureAsync(picture);
        return Ok(new MessageDto(PictureMessage.DeleteSucces));
    }
    #endregion

    //TODO IN PROD : Changer toute cette merde par un seul DTO avec un Array de byte pour la picture data
    /// <summary>
    /// Create and Add picture to Game
    /// </summary>
    /// <param name="pictureData">Picture File</param>
    /// <param name="gameId">Id of game to add picture</param>
    /// <param name="name">Name of picture</param>
    /// <param name="description">Description of Picture</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddPictureToGame/{gameId}")]
    public async Task<IActionResult> AddPictureToGame(IFormFile pictureData, [FromRoute] Guid gameId, [FromQuery] string name, string description)
    {
        AddPictureToGameDto dto = new()
        {
            GameId = gameId,
            Name = name,
            Description = description
        };
        ValidationResult validationResult = _addPictureToGameValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Game? game = await _gamePlatform.GetGameByIdAsync((Guid)dto.GameId!);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        await _picturePlatfrom.AddPictureToGameAsync(pictureData, dto, game);
        return Ok(new MessageDto(PictureMessage.SucessAddToGame));
    }

    /// <summary>
    /// Get all pictures of game
    /// </summary>
    /// <param name="gameId">Id of game</param>
    [ProducesResponseType(typeof(IEnumerable<ListPictureDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("GetPicturesByGameId/{gameId}")]
    public async Task<ActionResult<IEnumerable<ListPictureDto>>> GetPicturesByGameId([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        IEnumerable<Picture> pictures = await _picturePlatfrom.GetPicturesByGameIdAsync(game);

        List<FileContentResult> fileResults = new();
        foreach (Picture picture in pictures)
        {
            FileContentResult fileResult = new(picture.Data, "image/jpeg");
            fileResults.Add(fileResult);
        }

        return Ok(pictures.ToEnumerable_ListPictureDto());
    }
}
