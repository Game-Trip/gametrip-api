using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Models.Auth;

public class GameTripUserDto
{
    public GameTripUserDto(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    public string UserName { get; }
    public string Email { get; }
}

public static class GameTripUserDTOMapper
{
    public static GameTripUserDto ToDTO(this GameTripUser user) => new(user.UserName, user.Email);
}