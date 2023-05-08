using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LikeModels.Location;

namespace GameTrip.Domain.Extension;
public static class LikeLocationExtension
{
    public static LikedLocationDto ToDto(this IEnumerable<LikedLocation> likedLocations)
    {

        return new()
        {
            LikedLocationId = likedLocations.First().IdLikedLocation,
            LocationId = likedLocations.First().LocationId,
            UsersIds = likedLocations.Select(ll => ll.UserId),
            NbVote = likedLocations.Count(),
            MaxValue = likedLocations.OrderBy(ll => ll.Vote).First().Vote,
            MinValue = likedLocations.OrderBy(ll => ll.Vote).Last().Vote,
            AverageValue = likedLocations.Select(ll => ll.Vote).Average()
        };
    }

    public static IEnumerable<ListLikedLocationDto> ToListLikedLocationDto(this IEnumerable<LikedLocation> likedLocations)
    {
        return likedLocations.Select(ll => new ListLikedLocationDto()
        {
            LikedLocationId = ll.IdLikedLocation,
            LocationId = ll.LocationId,
            Location = ll.Location.ToLocationNameDto(),

        }).AsEnumerable();
    }
}
