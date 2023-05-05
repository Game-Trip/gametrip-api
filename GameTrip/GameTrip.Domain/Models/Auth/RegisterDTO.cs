using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Auth;

public class RegisterDTO
{
    public RegisterDTO(string username, string email, string password, string confirmPassword)
    {
        Username = username;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    [Required]
    public string Username { get; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; }
}