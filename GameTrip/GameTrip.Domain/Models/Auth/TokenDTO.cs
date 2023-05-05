using System.ComponentModel.DataAnnotations;

namespace GameTrip.Domain.Models.Auth;

public class TokenDto
{
    public TokenDto(string token, DateTime expirationDate)
    {
        Token = token;
        ExpirationDate = expirationDate;
    }

    public TokenDto(string token)
    {
        Token = token;
        ExpirationDate = null;
    }

    [Required]
    public string Token { get; set; }

    public DateTime? ExpirationDate { get; set; }
}