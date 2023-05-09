namespace GameTrip.Domain.Entities;

public class Picture
{
    public Guid IdPicture { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[]? Data { get; set; }
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }
    public Guid? GameId { get; set; }
    public Game? Game { get; set; }
}