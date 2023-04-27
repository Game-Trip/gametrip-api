using GameTrip.Domain.Entities;

namespace GameTrip.API.Models.Auth;

public class GameTripUserDTO
{
    public GameTripUserDTO(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    public string UserName { get; }
    public string Email { get; }
}

public static class GameTripUserDTOMapper
{
    public static GameTripUserDTO ToDTO(this GameTripUser user) => new(user.UserName, user.Email);
}