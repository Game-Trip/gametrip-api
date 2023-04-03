using GameTrip.Domain.Entities;
using GameTrip.Domain.Settings;
using GameTrip.EFCore;
using GameTrip.EFCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace GameTrip.API.Controllers
{
    [Route("")]
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

        ///// <summary>
        ///// Permet de register un user dans la DB
        ///// </summary>
        ///// <param name="discordId">id discord de l'utilisateur</param>
        ///// <param name="dto">Model de l'utilisateur</param>
        ///// <response code="400 + Message"></response>
        ///// <response code="200 + Message"></response>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("register/{discordId}")]
        //public async Task<IActionResult> Register([FromRoute] string discordId, [FromBody] RegisterDTO dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);


        //    ApiUser userExists = await userManager.FindByNameAsync($"{dto.Prenom}{dto.Nom}");
        //    if (userExists != null) return BadRequest("L'utilisateur existe déjà");

        //    Stage? stage = await _context.Stage.FirstOrDefaultAsync(s => s.Name == StageName.NA);
        //    ApiUser user = new ApiUser
        //    {
        //        UserName = $"{dto.Prenom}_{dto.Nom}",
        //        Email = dto.DiscordId,
        //        EmailConfirmed = false,
        //        Prenom = dto.Prenom,
        //        Nom = dto.Nom,
        //        Sexe = dto.Sexe,
        //        IdStage = stage.StageId,
        //        Argent = registrationSettings.defaultMoney,
        //        CreatedAt = DateTime.Now,
        //        Permis = PermisName.NA,
        //        Points = 0,
        //        NbSessions = 0,
        //        NbSessionsPermis = 0,
        //        NbSessionsPolice = 0,
        //    };

        //    IdentityResult? result = await userManager.CreateAsync(user, dto.Password);
        //    if (result.Succeeded == false) return BadRequest(result.Errors);



        //    string registrationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        //    registrationToken = HttpUtility.UrlEncode(registrationToken);

        //    EmailConfirmationTokenDTO tokenDTO = new() { token = registrationToken };

        //    //discord valide token stp :D
        //    //var url = $"{apiToBotSettings.baseURI}sendRegisterValidationButton/{user.Email}";
        //    var url = $"http://bot.guanajuato-roleplay.fr/sendRegisterValidationButton/{user.Email}";
        //    HttpClient client = new();
        //    string json = JsonSerializer.Serialize(tokenDTO);
        //    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync(url, data);

        //    return Ok("L'utilisateur a été crée et est en attente de validation");
        //}


        ///// <summary>
        ///// Permet de login un user dans la DB
        ///// </summary>
        ///// <param name="dto">Model de login d'un user</param>
        ///// <response code="400 + Message"></response>
        ///// <response code="401">Erreur de mdp ou id</response>
        ///// <response code="200">Token + date d'expiration</response>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    ApiUser? user = await userManager.FindByNameAsync(dto.Username);
        //    if (user != null && await userManager.CheckPasswordAsync(user, dto.Password))
        //    {
        //        IList<string>? userRoles = await userManager.GetRolesAsync(user);

        //        List<Claim> authClaims = new List<Claim>
        //        {
        //            new Claim("Name", user.UserName),
        //            new Claim("DiscordId", user.Email),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim("Roles", userRole));
        //        }

        //        SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));

        //        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(authClaims),
        //            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.DurationTime),
        //            Issuer = jwtSettings.ValidIssuer,
        //            Audience = jwtSettings.ValidAudience,
        //            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
        //        };

        //        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        //        //JwtSecurityToken token = new JwtSecurityToken(
        //        //    issuer: JWTSettings.ValidIssuer,
        //        //    audience: JWTSettings.ValidAudience,
        //        //    expires: DateTime.Now.AddMinutes(jwtSettings.DurationTime),
        //        //    claims: authClaims,
        //        //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        //);

        //        return Ok(new
        //        {
        //            token = tokenHandler.WriteToken(token),
        //            exipration = token.ValidTo
        //        });
        //    }
        //    else return Unauthorized();
        //}


        ///// <summary>
        ///// Teste la validiter d'un token
        ///// </summary>
        ///// <param name="token">token a check</param>
        ///// <response code="401">Token non valide || Pas la permission d'acceder a cette endpoint</response>
        ///// <response code="200">Token valide</response>
        //[HttpPost]
        //[Route("TokenTest")]
        //public async Task<IActionResult> TokenTest([FromBody] string token)
        //{
        //    SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidIssuer = jwtSettings.ValidIssuer,
        //            ValidAudience = jwtSettings.ValidAudience,
        //            IssuerSigningKey = authSigningKey
        //        }, out SecurityToken validatedToken);
        //    }
        //    catch
        //    {
        //        return Unauthorized();
        //    }
        //    return Ok();
        //}
    }
}
