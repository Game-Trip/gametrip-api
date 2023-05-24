namespace GameTrip.Domain.Entities;
public class RequestLocationUpdate
{
    public Guid IdRequestLocationUpdate { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<Game>? Games { get; set; }
}
