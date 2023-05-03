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

    public AuthController(UserManager<GameTripUser> userManager, IAuthPlatform authPlatform, IMailPlatform mailPlatform, IEmailProvider emailProvider)
    {
        _userManager = userManager;
        _authPlatform = authPlatform;
        _mailPlatform = mailPlatform;
        _emailProvider = emailProvider;
    }

    [HttpPost]
    [Route("Initialize")]
    public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer)
    {
        bool result = await dBInitializer.Initialize();
        string resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

        return Ok(resultMessage);
    }

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
            return Unauthorized(new ErrorResultDTO(UserError.FailedLogin));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<GameTripUserDTO>> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists is not null)
            return BadRequest(new ErrorResultDTO(UserError.MailAlreadyExist));
        userExists ??= await _userManager.FindByNameAsync(dto.Username);
        if (userExists is not null)
            return BadRequest(new ErrorResultDTO(UserError.UserNameAlreadyExist));

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

    [AllowAnonymous]
    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmMailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new ErrorResultDTO(UserError.NotFoundByMail));

        IdentityResult result = await _authPlatform.ConfirmEmailAsync(user, dto.Token);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, Roles.User);

        return result.Succeeded ? Ok() : Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("FrogotPassword")]
    public async Task<IActionResult> FrogotPassword([FromBody] FrogotPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest(new ErrorResultDTO(UserError.NotFoundByMail));

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

    [AllowAnonymous]
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return BadRequest(new ErrorResultDTO(UserError.NotFoundByMail));

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