namespace GameTrip.Domain.Models.Auth;

public class ForgotPasswordDto
{
    public ForgotPasswordDto(string email) => Email = email;

    public string Email { get; }
}
