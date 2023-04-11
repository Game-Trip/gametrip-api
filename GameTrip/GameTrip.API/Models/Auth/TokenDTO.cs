using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Auth;

public class TokenDTO
{
    public TokenDTO(string token, DateTime expirationDate)
    {
        Token = token;
        ExpirationDate = expirationDate;
    }

    [Required]
    public string Token { get; }

    public TokenDTO(string token)
    {
        Token = token;
    }

    public DateTime? ExpirationDate { get; }
}