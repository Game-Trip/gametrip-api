using GameTrip.API.Models.Auth;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Errors;
using GameTrip.Domain.Models.Email;
using GameTrip.Domain.Models.Email.Template;
using GameTrip.Domain.Settings;
using GameTrip.EFCore.Data;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GameTrip.API.Controllers;
/// <summary>
/// The auth controller.
/// </summary>

[Consumes("application/json")]
[Produces("application/json")]
[Route("[controller]")]
[Authorize]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<GameTripUser> _userManager;
    private readonly IAuthPlatform _authPlatform;
    private readonly IMailPlatform _mailPlatform;
    private readonly IEmailProvider _emailProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="authPlatform">The auth platform.</param>
    /// <param name="mailPlatform">The mail platform.</param>
    /// <param name="emailProvider">The email provider.</param>
    public AuthController(UserManager<GameTripUser> userManager, IAuthPlatform authPlatform, IMailPlatform mailPlatform, IEmailProvider emailProvider)
    {
        _userManager = userManager;
        _authPlatform = authPlatform;
        _mailPlatform = mailPlatform;
        _emailProvider = emailProvider;
    }

    /// <summary>
    /// Add first user and roles in DB if not exist
    /// </summary>
    /// <remarks>Need to be login with bearer token to use this endpoint</remarks>
    /// <param name="dBInitializer">Class who provide method to init DB</param>
    /// <returns>IActionResul</returns>
    /// <response code="200">With message who indicate if DB Create or already exist</response>
    [HttpPost]
    [Route("Initialize")]
    public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer)
    {
        bool result = await dBInitializer.Initialize();
        string resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

        return Ok(resultMessage);
    }

    /// <summary>
    /// EndPoint to login user with username or email and password
    /// </summary>
    /// <param name="dto">Class who containt Identifier and password properties</param>
    /// <returns>Token DTO with bearer token and this expiration date</returns>
    /// <response code="401">If identifier is not registred or password not correct</response>
    [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorReturnDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByNameAsync(dto.Username);
        user ??= await _userManager.FindByEmailAsync(dto.Username);

        if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = await _authPlatform.CreateTokenAsync(user);
            return Ok(new TokenDTO(tokenHandler.WriteToken(token), token.ValidTo));
        }
        else
            return Unauthorized(new ErrorReturnDTO(UserError.FailedLogin));
    }

    /// <summary>
    /// EndPoint to register user with Username, Email and Password
    /// </summary>
    /// <returns>GameTripUserDTO</returns>
    /// <exception cref="FileNotFoundException">Register template mail not found</exception>
    /// <response code="200">The user is registered and will receive an account confirmation email</response>
    /// <response code="400">If username or email already exist</response>
    [ProducesResponseType(typeof(GameTripUserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorReturnDTO), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<GameTripUserDTO>> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists is not null)
            return BadRequest(new ErrorReturnDTO(UserError.MailAlreadyExist));
        userExists ??= await _userManager.FindByNameAsync(dto.Username);
        if (userExists is not null)
            return BadRequest(new ErrorReturnDTO(UserError.UserNameAlreadyExist));

        GameTripUser user = new()
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        IdentityResult? result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded is false)
            return BadRequest(result.Errors);

        string confirmationLink = await _authPlatform.GenerateEmailConfirmationLinkAsync(user);

        string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.Register)!;
        if (emailTemplateText is null)
            throw new FileNotFoundException(TemplateErrors.TemplateRegisterNotFound.ToString());

        emailTemplateText = emailTemplateText.Replace("{0}", user.UserName);
        emailTemplateText = emailTemplateText.Replace("{1}", confirmationLink);

        MailDTO mailDTO = new()
        {
            Name = user.UserName,
            Email = user.Email,
            Subject = "Bienvenue sur GameTrip",
            Body = emailTemplateText
        };

        await _mailPlatform.SendMailAsync(mailDTO);

        return Ok(user.ToDTO());
    }

    /// <summary>
    /// Confirms the email.
    /// </summary>
    /// <param name="dto">The dto.</param>
    /// <returns>A Task.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmMailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new ErrorReturnDTO(UserError.NotFoundByMail));

        IdentityResult result = await _authPlatform.ConfirmEmailAsync(user, dto.Token);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, Roles.User);

        return result.Succeeded ? Ok() : Unauthorized();
    }

    /// <summary>
    /// Frogots the password.
    /// </summary>
    /// <param name="dto">The dto.</param>
    /// <returns>A Task.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("FrogotPassword")]
    public async Task<IActionResult> FrogotPassword([FromBody] FrogotPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest(new ErrorReturnDTO(UserError.NotFoundByMail));

        string resetPasswordLink = await _authPlatform.GeneratePasswordResetLinkAsync(user);

        string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.FrogotPassword)!;
        if (emailTemplateText is null)
            throw new FileNotFoundException(TemplateErrors.TemplateFrogotPasswordNotFound.ToString());

        emailTemplateText = emailTemplateText.Replace("{0}", user.UserName);
        emailTemplateText = emailTemplateText.Replace("{1}", resetPasswordLink);

        MailDTO mailDTO = new()
        {
            Name = user.UserName,
            Email = user.Email,
            Subject = "Changement de mot de passe",
            Body = emailTemplateText
        };

        await _mailPlatform.SendMailAsync(mailDTO);

        return Ok();
    }

    /// <summary>
    /// Resets the password.
    /// </summary>
    /// <param name="dto">The dto.</param>
    /// <returns>A Task.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new ErrorReturnDTO(UserError.NotFoundByMail));

        IdentityResult? result = await _authPlatform.ResetPasswordAsync(user, dto.Password, dto.Token);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
        }

        return result.Succeeded ? Ok() : BadRequest(ModelState);
    }
}