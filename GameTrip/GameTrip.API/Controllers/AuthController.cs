using GameTrip.API.Models.Auth;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Settings;
using GameTrip.EFCore.Data;
using GameTrip.Platform;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

namespace GameTrip.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<GameTripUser> _userManager;
        private readonly IAuthPlatform _authPlatform;

        public AuthController(UserManager<GameTripUser> userManager, IAuthPlatform authPlatform)
        {
            _userManager = userManager;
            _authPlatform = authPlatform;
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
            var result = await dBInitializer.Initialize();
            var resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

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
            if (user == null) user = await _userManager.FindByEmailAsync(dto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
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
        public async Task<IActionResult> TokenTest([FromBody] string token)
        {
            bool isValid = _authPlatform.TestToken(token);
            if (isValid)
                return Ok();
            else
                return Unauthorized();
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

            GameTripUser user = new GameTripUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            IdentityResult? result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded == false) return BadRequest(result.Errors);

            string registrationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            registrationToken = HttpUtility.UrlEncode(registrationToken);

            //EmailConfirmationTokenDTO tokenDTO = new() { token = registrationToken };

            ////discord valide token stp :D
            ////var url = $"{apiToBotSettings.baseURI}sendRegisterValidationButton/{user.Email}";
            //var url = $"http://bot.guanajuato-roleplay.fr/sendRegisterValidationButton/{user.Email}";
            //HttpClient client = new();
            //string json = JsonSerializer.Serialize(tokenDTO);
            //StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await client.PostAsync(url, data);

            return Ok(user.ToDTO());
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("No account has been created with this email");

            IdentityResult? result = await _authPlatform.ResetPasswordAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("The password has been successfully changed");
        }
    }
}