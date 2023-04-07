using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models.Login;

public class LoginDTO
{
    public LoginDTO(string username, string password)
    {
        Username = username;
        Password = password;
    }

    [Required]
    public string Username { get; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; }
}
