﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameTrip.Platform
{
    public class AuthPlatform : IAuthPlatform
    {
        #region Properties

        private readonly UserManager<GameTripUser> _userManager;
        private readonly JWTSettings _jwtSettings;

        #endregion Properties

        #region Constructor

        public AuthPlatform(UserManager<GameTripUser> userManager, JWTSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        #endregion Constructor

        #region Public Methods

        public async Task<SecurityToken> CreateTokenAsync(GameTripUser user)
        {
            IList<string>? userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = new()
            {
                    new Claim("User", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (string userRole in userRoles)
            {
                authClaims.Add(new Claim("Roles", userRole));
            }

            SymmetricSecurityKey authSigningKey = new(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.DurationTime),
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.CreateToken(tokenDescriptor); ;
        }

        public async Task<IdentityResult?> ResetPasswordAsync(GameTripUser user, string password)
        {
            //TEMPORAIRE LE TEMPS DE SETUP LE SEND DE TOKEN PAR EMAIL
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, resetToken, password);
        }

        public bool TestToken(string token)
        {
            SymmetricSecurityKey authSigningKey = new(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            JwtSecurityTokenHandler tokenHandler = new();
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
                return false;
            }
            return true;
        }

        #endregion Public Methods
    }
}