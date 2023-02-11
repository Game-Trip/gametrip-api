using Microsoft.AspNetCore.Identity;

namespace GameTrip.API.Data;

public class GameTripUser : IdentityUser<Guid>
{
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<LikedGame>? LikedGames { get; set; }
    public ICollection<LikedLocation>? LikedLocations { get; set; }
}