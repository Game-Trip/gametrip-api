namespace GameTrip.Domain.Models.Auth;

public class RegisterDto
{
    public RegisterDto(string username, string email, string password, string confirmPassword)
    {
        Username = username;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string Username { get; }
    public string Email { get; }
    public string Password { get; }
    public string ConfirmPassword { get; }
}