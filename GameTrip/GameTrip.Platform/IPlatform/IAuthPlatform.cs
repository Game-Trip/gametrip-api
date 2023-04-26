using GameTrip.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GameTrip.Platform.IPlatform;

public interface IAuthPlatform
{
    public Task<SecurityToken> CreateTokenAsync(GameTripUser user);

    public bool TestToken(string token);

    public Task<IdentityResult?> ResetPasswordAsync(GameTripUser user, string password, string token);
    Task<string> GenerateEmailConfirmationLinkAsync(GameTripUser user);
    Task<string> GeneratePasswordResetLinkAsync(GameTripUser user);
    Task<IdentityResult> ConfirmEmailAsync(GameTripUser user, string token);
}