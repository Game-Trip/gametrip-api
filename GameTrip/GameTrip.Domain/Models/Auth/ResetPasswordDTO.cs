namespace GameTrip.Domain.Models.Auth;

public class ResetPasswordDto
{
    public ResetPasswordDto(string email, string password, string passwordConfirmation, string token)
    {
        Email = email;
        Password = password;
        PasswordConfirmation = passwordConfirmation;
        Token = token;
    }

    public string Email { get; }
    public string Password { get; }
    public string PasswordConfirmation { get; }
    public string Token { get; set; }
}