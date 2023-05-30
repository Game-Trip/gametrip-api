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

            Comments = user.Comments?.ToList_ListCommentDto(),
            LikedGames = user.LikedGames?.ToEnumerable_ListLikedGameDto(),
            LikedLocations = user.LikedLocations?.ToEnumerable_ListLikedLocationDto()
        };
    }

    public static GameTripUserDtoName ToGameTripUserDtoName(this GameTripUser user)
    {
        return new()
        {
            UserId = user.Id,
            UserName = user.UserName
        };
    }
}
