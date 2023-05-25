namespace GameTrip.Domain.Models.GameModels;
public class ListGameUpdateRequest
{
    public Guid GameId { get; set; }
    public GetGameDto? Game { get; set; }

    public IEnumerable<GameUpdateRequestNameDto>? RequestUpdate { get; set; }
}
