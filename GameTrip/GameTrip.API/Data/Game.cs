namespace GameTrip.API.Data;

public class Game
{
    public Guid IdGame { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Editor { get; set; }
    public long RealaseDate { get; set; }

    public ICollection<Location>? Locations { get; set; }
    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<LikedGame>? LikedGames { get; set; }
}