namespace GameTrip.Domain.Entities;
public class RequestGameUpdate
{
    public Guid IdRequestGameUpdate { get; set; }
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }

    public ICollection<Location>? Locations { get; set; }
    public ICollection<Picture>? Pictures { get; set; }
}
