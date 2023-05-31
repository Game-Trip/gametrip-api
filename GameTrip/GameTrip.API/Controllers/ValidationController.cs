using GameTrip.Domain.Entities;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ValidationController : ControllerBase
{
    private readonly UserManager<GameTripUser> _userManager;
    private readonly ICommentPlatform _commentPlatform;
    private readonly IGamePlatform _gamePlatform;
    private readonly ILocationPlarform _locationPlatform;
    private readonly IPicturePlatfrom _picturePlatfrom;

    public ValidationController(UserManager<GameTripUser> userManager, ICommentPlatform commentPlatform, IGamePlatform gamePlatform, ILocationPlarform locationPlatform, IPicturePlatfrom picturePlatfrom)
    {
        _userManager = userManager;
        _commentPlatform = commentPlatform;
        _gamePlatform = gamePlatform;
        _locationPlatform = locationPlatform;
        _picturePlatfrom = picturePlatfrom;
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("SwitchCommentValidateState/{commentId}/{userId}")]
    public async Task<ActionResult<MessageDto>> SwitchCommentValidateState([FromRoute] Guid commentId, [FromRoute] Guid userId)
    {
        Comment? comment = await _commentPlatform.GetCommentByIdAsync(commentId);
        if (comment is null)
            return NotFound(new MessageDto(CommentMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (comment.User != user)
            return BadRequest(new MessageDto(CommentMessage.UserNotAuthor));

        await _commentPlatform.SwitchValidateStatusCommentAsync(comment);

        if (comment.IsValidate)
            return new MessageDto(CommentMessage.NowValidate);
        else
            return new MessageDto(CommentMessage.NowNotValidate);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("SwitchGameValidateState/{gameId}/{userId}")]
    public async Task<ActionResult<MessageDto>> SwitchGameValidateState([FromRoute] Guid gameId, [FromRoute] Guid userId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (game.Author != user)
            return BadRequest(new MessageDto(GameMessage.UserNotAuthor));

        await _gamePlatform.SwitchValidateStatusGameAsync(game);

        if (game.IsValidate)
            return new MessageDto(GameMessage.NowValidate);
        else
            return new MessageDto(GameMessage.NowNotValidate);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("SwitchLocationValidateState/{locationId}/{userId}")]
    public async Task<ActionResult<MessageDto>> SwitchLocationValidateState([FromRoute] Guid locationId, [FromRoute] Guid userId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (location.Author != user)
            return BadRequest(new MessageDto(LocationMessage.UserNotAuthor));

        await _locationPlatform.SwitchValidateStatusLocationAsync(location);

        if (location.IsValid)
            return new MessageDto(LocationMessage.NowValidate);
        else
            return new MessageDto(LocationMessage.NowNotValidate);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [Route("SwitchPictureValidateState/{pictureId}/{userId}")]
    public async Task<ActionResult<MessageDto>> SwitchPictureValidateState([FromRoute] Guid pictureId, [FromRoute] Guid userId)
    {
        Picture? picture = await _picturePlatfrom.GetPictureByIdAsync(pictureId);
        if (picture is null)
            return NotFound(new MessageDto(PictureMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (picture.Author != user)
            return BadRequest(new MessageDto(PictureMessage.UserNotAuthor));

        await _picturePlatfrom.SwitchValidateStatusPictureAsync(picture);

        if (picture.IsValidate)
            return new MessageDto(PictureMessage.NowValidate);
        else
            return new MessageDto(PictureMessage.NowNotValidate);
    }
}
