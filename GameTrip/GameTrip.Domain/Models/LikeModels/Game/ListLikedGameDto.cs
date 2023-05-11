using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Models.LikeModels.Game;
public class ListLikedGameDto
{
    public Guid? LikedGameId { get; set; }
    public Guid? GameId { get; set; }
    public GameNameDto? Game { get; set; }
}
