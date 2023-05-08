namespace GameTrip.Domain.Models.LikeModels.Game;
public class AddLikeGameDto
{
    public Guid? GameId { get; set; }
    public Guid? UserId { get; set; }
    public decimal? Value { get; set; }
}
