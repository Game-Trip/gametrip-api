namespace GameTrip.Domain.Entities;

public class Game
{
    public Guid IdGame { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
    public bool IsValidate { get; set; }

    public Guid AuthorId { get; set; }
    public GameTripUser? Author { get; set; }

    public ICollection<Location>? Locations { get; set; }
    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<LikedGame>? LikedGames { get; set; }
}