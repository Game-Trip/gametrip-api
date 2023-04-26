using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Auth;

public class FrogotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; }

    public FrogotPasswordDto(string email)
    {
        Email = email;
    }
}
