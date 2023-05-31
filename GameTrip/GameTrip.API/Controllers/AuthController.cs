using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Errors;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;
using GameTrip.Domain.Models.Email;
using GameTrip.Domain.Models.Email.Template;
using GameTrip.Domain.Settings;
using GameTrip.EFCore.Data;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace GameTrip.API.Controllers;
/// <summary>
/// The auth controller.
/// </summary>

[Route("[controller]")]
[Authorize]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<GameTripUser> _userManager;
    private readonly IAuthPlatform _authPlatform;
    private readonly IMailPlatform _mailPlatform;
    private readonly IEmailProvider _emailProvider;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<ConfirmMailDto> _confirmMailValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;

    public AuthController(UserManager<GameTripUser> userManager, IAuthPlatform authPlatform, IMailPlatform mailPlatform, IEmailProvider emailProvider, IValidator<ForgotPasswordDto> forgotPasswordValidator, IValidator<ResetPasswordDto> resetPasswordValidator, IValidator<ConfirmMailDto> confirmMailValidator, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
    {
        _userManager = userManager;
        _authPlatform = authPlatform;
        _mailPlatform = mailPlatform;
        _emailProvider = emailProvider;
        _forgotPasswordValidator = forgotPasswordValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _confirmMailValidator = confirmMailValidator;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Initialize")]
    public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer)
    {
        bool result = await dBInitializer.Initialize();
        string resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

        return Ok(resultMessage);
    }

    /// <summary>
    /// Authenticate a user
    /// </summary>
    /// <param name="dto">LoginDto</param>
    /// <remarks>
    /// {
    ///   "username": "Dercraker",
    ///   "password": "NMdRx$HqyT8jX6"
    /// }
    /// </remarks>
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.Unauthorized)]
    [AllowAnonymous]
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
    {
        ValidationResult resultValidation = _loginValidator.Validate(dto);
        if (!resultValidation.IsValid)
        {
            resultValidation.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? user = await _userManager.FindByNameAsync(dto.Username);
        user ??= await _userManager.FindByEmailAsync(dto.Username);

        if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = await _authPlatform.CreateTokenAsync(user);
            return new TokenDto(tokenHandler.WriteToken(token), token.ValidTo);
        }
        else
            return Unauthorized(new MessageDto(UserMessage.FailedLogin));
    }

    /// <summary>
    /// Register a user
    /// </summary>
    /// <param name="dto">RegisterDto</param>
    [ProducesResponseType(typeof(GameTripUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<GameTripUserDto>> Register([FromBody] RegisterDto dto)
    {
        ValidationResult resultValidation = _registerValidator.Validate(dto);
        if (!resultValidation.IsValid)
        {
            resultValidation.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists is not null)
            return BadRequest(new MessageDto(UserMessage.MailAlreadyExist));
        userExists ??= await _userManager.FindByNameAsync(dto.Username);
        if (userExists is not null)
            return BadRequest(new MessageDto(UserMessage.UserNameAlreadyExist));

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
            throw new FileNotFoundException(TemplateMessage.TemplateRegisterNotFound.ToString());

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

        return user.ToGameTripUserDto();
    }

    /// <summary>
    /// Confirms the email of provided user.
    /// </summary>
    /// <param name="dto">ConfirmMailDto</param>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [AllowAnonymous]
    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmMailDto dto)
    {
        ValidationResult resultValidation = _confirmMailValidator.Validate(dto);
        if (!resultValidation.IsValid)
        {
            resultValidation.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new MessageDto(UserMessage.NotFoundByMail));

        IdentityResult result = await _authPlatform.ConfirmEmailAsync(user, dto.Token);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, Roles.User);

        return result.Succeeded ? Ok() : Unauthorized();
    }

    /// <summary>
    /// Send Forgot Password Mail to user
    /// </summary>
    /// <param name="dto">ForgotPasswordDto</param>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        ValidationResult result = _forgotPasswordValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest(new MessageDto(UserMessage.NotFoundByMail));

        string resetPasswordLink = await _authPlatform.GeneratePasswordResetLinkAsync(user);

        string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.ForgotPassword)!;
        if (emailTemplateText is null)
            throw new FileNotFoundException(TemplateMessage.TemplateForgotPasswordNotFound.ToString());

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
    /// change the user's password
    /// </summary>
    /// <param name="dto">ResetPasswrdDto</param>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.Unauthorized)]
    [AllowAnonymous]
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        ValidationResult resultValidation = _resetPasswordValidator.Validate(dto);
        if (!resultValidation.IsValid)
        {
            resultValidation.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new MessageDto(UserMessage.NotFoundByMail));

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
