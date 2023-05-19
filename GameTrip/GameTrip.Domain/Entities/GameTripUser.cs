using Microsoft.AspNetCore.Identity;

namespace GameTrip.Domain.Entities;

public class GameTripUser : IdentityUser<Guid>
{
    public ICollection<Game>? CreatedGame { get; set; }
    public ICollection<Location>? CreatedLocation { get; set; }
    public ICollection<Picture>? CreatedPictures { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<LikedGame>? LikedGames { get; set; }
    public ICollection<LikedLocation>? LikedLocations { get; set; }
}