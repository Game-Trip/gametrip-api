using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;
using GameTrip.Domain.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<GameTripUser> _userManager;
    private readonly IValidator<UpdateGameTripUserDto> _updateGameTripUserValidator;

    public UserController(UserManager<GameTripUser> userManager, IValidator<UpdateGameTripUserDto> updateGameTripUserValidator)
    {
        _userManager = userManager;
        _updateGameTripUserValidator = updateGameTripUserValidator;
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="userId">Id of user</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("Id/{userId}")]
    public async Task<ActionResult<GameTripUserDto>> GetUserById([FromRoute] Guid userId)
    {
        GameTripUser? user = await _userManager.Users.Include(u => u.Comments).Include(u => u.LikedGames).Include(u => u.LikedLocations).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));
        return user.ToGameTripUserDto();
    }

    /// <summary>
    /// Get user by Name
    /// </summary>
    /// <param name="userName">Name of user</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("Name/{userName}")]
    public async Task<ActionResult<GameTripUserDto>> GetUserByName([FromRoute] string userName)
    {
        GameTripUser? user = await _userManager.Users.Include(u => u.Comments).Include(u => u.LikedGames).Include(u => u.LikedLocations).FirstOrDefaultAsync(u => u.UserName == userName);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundByUserName));
        return user.ToGameTripUserDto();
    }

    /// <summary>
    /// Get user by Mail
    /// </summary>
    /// <param name="userMail">Mail of user</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("Email/{userMail}")]
    public async Task<ActionResult<GameTripUserDto>> GetUserByMail([FromRoute] string userMail)
    {
        GameTripUser? user = await _userManager.Users.Include(u => u.Comments).Include(u => u.LikedGames).Include(u => u.LikedLocations).FirstOrDefaultAsync(u => u.Email == userMail);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundByMail));
        return user.ToGameTripUserDto();
    }

    /// <summary>
    /// Delete User By Id
    /// </summary>
    /// <param name="userId">Id of user</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteUserById([FromRoute] Guid userId)
    {
        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundByMail));

        await _userManager.DeleteAsync(user);
        return Ok();
    }

    /// <summary>
    /// Update User
    /// </summary>
    /// <param name="userId">Id of user to delete</param>
    /// <param name="dto">UpdateGameTripUserDto</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    [Route("{userId}")]
    public async Task<ActionResult<GameTripUserDto>> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateGameTripUserDto dto)
    {
        ValidationResult validationResult = _updateGameTripUserValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? Entity = await _userManager.FindByIdAsync(userId.ToString());
        if (Entity is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        await _userManager.UpdateAsync(Entity);

        GameTripUser? user = await _userManager.Users.Include(u => u.Comments).Include(u => u.LikedGames).Include(u => u.LikedLocations).FirstOrDefaultAsync(u => u.Id == dto.Id);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));
        return user.ToGameTripUserDto();
    }
}
