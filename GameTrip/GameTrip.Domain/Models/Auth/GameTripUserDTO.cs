using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LikeModels.Game;
using GameTrip.Domain.Models.LikeModels.Location;

namespace GameTrip.Domain.Models.Auth;

public class GameTripUserDto
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }

    public IEnumerable<Comment>? Comments { get; set; }
    public IEnumerable<ListLikedGameDto>? LikedGames { get; set; }
    public IEnumerable<ListLikedLocationDto>? LikedLocations { get; set; }
}