using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Auth;

public class ConfirmMailDto
{
    public ConfirmMailDto(string token, string email)
    {
        Token = token;
        Email = email;
    }

    [Required]
    public string Token { get; }

    [Required]
    public string Email { get; }
}
