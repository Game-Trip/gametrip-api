using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Models.LikeModels.Game;
public class LikedGameDto
{
    public Guid? LikedGameId { get; set; }
    public Guid? GameId { get; set; }
    public GameNameDto? Game { get; set; }
    public IEnumerable<Guid>? UsersIds { get; set; }
    public int? NbVote { get; set; }
    public decimal? MaxValue { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? AverageValue { get; set; }
}
