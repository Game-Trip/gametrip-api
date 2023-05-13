namespace GameTrip.Domain.Models.Auth;
public class UpdateGameTripUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
