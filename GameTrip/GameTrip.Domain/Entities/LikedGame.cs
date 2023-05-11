namespace GameTrip.Domain.Entities;

public class LikedGame
{
    public Guid IdLikedGame { get; set; }
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
    public Guid UserId { get; set; }
    public GameTripUser? User { get; set; }
    public decimal Vote { get; set; }
}