using GameTrip.API.Models.Auth;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Settings;
using GameTrip.EFCore;
using GameTrip.EFCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace GameTrip.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GameTripContext _context;
        private readonly UserManager<GameTripUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly JWTSettings _jwtSettings;

        public AuthController(GameTripContext context, UserManager<GameTripUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, JWTSettings jwtSettings)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        /// Initialise les table avec les rôles et l'utilisateur Admin
        /// </summary>
        /// <response code="200 + Message"></response
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
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            GameTripUser? user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) user = await _userManager.FindByEmailAsync(dto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                IList<string>? userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>
                {
                    new Claim("User", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim("Roles", userRole));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(authClaims),
                    Expires = DateTime.UtcNow.AddHours(_jwtSettings.DurationTime),
                    Issuer = _jwtSettings.ValidIssuer,
                    Audience = _jwtSettings.ValidAudience,
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    exipration = token.ValidTo
                });
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
        [AllowAnonymous]
        [Route("TokenTest")]
        public async Task<IActionResult> TokenTest([FromBody] string token)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    ValidAudience = _jwtSettings.ValidAudience,
                    IssuerSigningKey = authSigningKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return Unauthorized();
            }
            return Ok();
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
    }
}