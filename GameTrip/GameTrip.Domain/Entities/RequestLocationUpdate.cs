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
    public Guid? IdPicture { get; set; }
    public Picture? Picture { get; set; }
    public bool? isAddedPicture { get; set; }
    public Guid? IdGame { get; set; }
    public Game? Game { get; set; }
    public bool? isAddedGame { get; set; }
}
