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

    public Guid? IdLocation { get; set; }
    public Location? Location { get; set; }
    public bool? isAddedLocation { get; set; }
    public Guid? IdPicture { get; set; }
    public Picture? Picture { get; set; }
    public bool? isAddedPicture { get; set; }
}
