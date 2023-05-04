using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Models.GameModels;
public class GameDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }

    public ICollection<Location>? Locations { get; set; }
    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<LikedGame>? LikedGames { get; set; }
}
