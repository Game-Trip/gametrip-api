namespace GameTrip.Domain.Models.Auth;

public class ConfirmMailDto
{
    public ConfirmMailDto(string token, string email)
    {
        Token = token;
        Email = email;
    }

    public string Token { get; }
    public string Email { get; }
}
