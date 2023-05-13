using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.Domain.Extension;
public static class GameTripUserExtension
{
    public static GameTripUserDto ToGameTripUserDto(this GameTripUser user)
    {
        return new()
        {
            UserName = user.UserName,
            Email = user.Email,
            UserId = user.Id,

            Comments = user.Comments,
            LikedGames = user.LikedGames?.ToEnumerable_ListLikedGameDto(),
            LikedLocations = user.LikedLocations?.ToEnumerable_ListLikedLocationDto()
        };
    }
}
