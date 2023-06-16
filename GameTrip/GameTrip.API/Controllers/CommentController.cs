using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Comment;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ILocationPlarform _locationPlatform;
    private readonly UserManager<GameTripUser> _userManager;
    private readonly IValidator<AddCommentToLocationDto> _addCommentToLocationValidator;
    private readonly IValidator<UpdateCommentDto> _updateCommentValidator;
    private readonly ICommentPlatform _commentPlatform;

    public CommentController(ILocationPlarform locationPlatform, UserManager<GameTripUser> userManager, IValidator<AddCommentToLocationDto> addCommentToLocationValidator, ICommentPlatform commentPlatform, IValidator<UpdateCommentDto> updateCommentValidator)
    {
        _locationPlatform = locationPlatform;
        _userManager = userManager;
        _addCommentToLocationValidator = addCommentToLocationValidator;
        _commentPlatform = commentPlatform;
        _updateCommentValidator = updateCommentValidator;
    }

    /// <summary>
    /// Add Comment To location
    /// </summary>
    /// <param name="locationId">Id of location where add comment</param>
    /// <param name="dto">AddCommentToLocationDto</param>
    /// <param name="froce">Force Validation for this comment</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("Add/{locationId}")]
    public async Task<ActionResult<MessageDto>> AddCommentToLocation([FromRoute] Guid locationId, [FromBody] AddCommentToLocationDto dto, [Optional][FromQuery] bool froce)
    {
        ValidationResult validationResult = _addCommentToLocationValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        if (locationId != dto.LocationId)
            return BadRequest(new MessageDto(CommentMessage.LocationIdAndIdInDtoNotEqual));

        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        await _commentPlatform.AddCommentToLocationAsync
            (location, user, dto, froce);
        return new MessageDto(CommentMessage.SuccessCreate);
    }

    /// <summary>
    /// Remove Comment By Id
    /// </summary>
    /// <param name="commentId">Id of comment to be removed</param>
    /// <response code="204">Comment has been deleted</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpDelete]
    [Route("Remove/{commentId}")]
    public async Task<IActionResult> RemoveCommentFromLocation([FromRoute] Guid commentId)
    {
        Comment? comment = await _commentPlatform.GetCommentByIdAsync(commentId);
        if (comment is null)
            return NotFound(new MessageDto(CommentMessage.NotFoundById));

        await _commentPlatform.DeleteAsync(comment);

        return NoContent();
    }

    /// <summary>
    /// Get All Comment By Location
    /// </summary>
    /// <param name="locationId">Id of location related of Comments</param>
    [ProducesResponseType(typeof(IEnumerable<ListCommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [AllowAnonymous]
    [HttpGet]
    [Route("Location/{locationId}")]
    public async Task<ActionResult<IEnumerable<ListCommentDto>>> GetAllCommentByLocation([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        IEnumerable<Comment>? comments = _commentPlatform.GetCommentAllByLocationId(location.IdLocation);
        if (!comments.Any())
            return NoContent();

        return comments.ToList_ListCommentDto();
    }

    /// <summary>
    /// Get All Comment By User
    /// </summary>
    /// <param name="userId">Id of User related of Comment</param>
    [ProducesResponseType(typeof(List<ListCommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("User/{userId}")]
    public async Task<ActionResult<List<ListCommentDto>>> GetAllCommentByUser([FromRoute] Guid userId)
    {
        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        IEnumerable<Comment>? comments = _commentPlatform.GetCommentAllByUserId(user.Id);
        if (!comments.Any())
            return NoContent();

        return comments.ToList_ListCommentDto();
    }

    /// <summary>
    /// Get Comment By Id
    /// </summary>
    /// <param name="commentId">Id of wanted Comment</param>
    [ProducesResponseType(typeof(IEnumerable<ListCommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetCommentById([FromRoute] Guid commentId)
    {
        Comment? comment = await _commentPlatform.GetCommentByIdAsync(commentId);
        if (comment is null)
            return NotFound(new MessageDto(CommentMessage.NotFoundById));

        return comment.ToCommentDto();
    }

    [ProducesResponseType(typeof(IEnumerable<ListCommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [Authorize(Roles = Roles.User)]
    [HttpPut]
    [Route("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetCommentById([FromRoute] Guid commentId, [FromBody] UpdateCommentDto dto)
    {
        ValidationResult validationResult = _updateCommentValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Comment? Entity = await _commentPlatform.GetCommentByIdAsync(commentId);
        if (Entity is null)
            return NotFound(new MessageDto(CommentMessage.NotFoundById));

        await _commentPlatform.UpdateCommentAsync(Entity, dto);

        Comment? comment = await _commentPlatform.GetCommentByIdAsync(commentId);

        return comment.ToCommentDto();
    }
}
