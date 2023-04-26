using GameTrip.API.Models.Auth;
using GameTrip.Domain.Entities;
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

namespace GameTrip.API.Controllers
{
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

        /// <summary>
        /// Initialise les table avec les rôles et l'utilisateur Admin
        /// </summary>
        /// <response code="200 + Message"></response>
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
        /// Permet de login un user dans la DB
        /// </summary>
        /// <param name="dto">Model de login d'un user</param>
        /// <response code="400 + Message"></response>
        /// <response code="401">Erreur de mdp ou username</response>
        /// <response code="200">Token + date d'expiration</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser? user = await _userManager.FindByNameAsync(dto.Username);
            user ??= await _userManager.FindByEmailAsync(dto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                JwtSecurityTokenHandler tokenHandler = new();
                SecurityToken token = await _authPlatform.CreateTokenAsync(user);
                return Ok(new TokenDTO(tokenHandler.WriteToken(token), token.ValidTo));
            }
            else return Unauthorized();
        }

        /// <summary>
        /// Teste la validiter d'un token
        /// </summary>
        /// <param name="token">token a check</param>
        /// <response code="401">Token non valide || Pas la permission d'acceder a cette endpoint</response>
        /// <response code="200">Token valide</response>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("TokenTest")]
        public IActionResult TokenTest([FromBody] string token)
        {
            bool isValid = _authPlatform.TestToken(token);
            return isValid ? Ok() : Unauthorized();
        }

        /// <summary>
        /// Permet de register un user dans la DB
        /// </summary>
        /// <param name="dto">Model de l'utilisateur</param>
        /// <response code="400 + Message"></response>
        /// <response code="200 + Message"></response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<GameTripUserDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null) return BadRequest("This mail is already taken");

            GameTripUser user = new()
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            IdentityResult? result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded == false) return BadRequest(result.Errors);

            string registrationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token = registrationToken, email = user.Email }, Request.Scheme);

            string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.Register)!;
            if (emailTemplateText == null) throw new FileNotFoundException();

            emailTemplateText = emailTemplateText.Replace("{0}", user.UserName);
            emailTemplateText = emailTemplateText.Replace("{1}", confirmationLink);

            MailDTO mailDTO = new()
            {
                Name = "Dercraker",
                Email = "antoine.capitain@gmail.com",
                Subject = "Bienvenue sur GameTrip",
                Body = emailTemplateText
            };

            await _mailPlatform.SendMailAsync(mailDTO);

            return Ok(user.ToDTO());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest();

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Redirect("portainer.game-trip.fr/") : Unauthorized();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("No account has been created with this email");

            IdentityResult? result = await _authPlatform.ResetPasswordAsync(user, dto.Password);
            return !result.Succeeded ? BadRequest(result.Errors) : Ok("The password has been successfully changed");
        }
    }
}