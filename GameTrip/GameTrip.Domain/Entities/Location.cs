namespace GameTrip.Domain.Entities;

public class Location
{
    public Guid IdLocation { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsValid { get; set; }

    public Guid AuthorId { get; set; }
    public GameTripUser? Author { get; set; }

    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<Game>? Games { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<LikedLocation>? LikedLocations { get; set; }
    public ICollection<RequestLocationUpdate>? RequestLocationUpdates { get; set; }
}