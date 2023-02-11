namespace GameTrip.API.Data;

public class Comment
{
    public Guid IdComment { get; set; }
    public string Text { get; set; }

    public Guid UserId { get; set; }
    public GameTripUser? User { get; set; }

    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
}