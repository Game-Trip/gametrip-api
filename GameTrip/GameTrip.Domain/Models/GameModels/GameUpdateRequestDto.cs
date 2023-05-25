namespace GameTrip.Domain.Models.GameModels;
public class GameUpdateRequestDto
{
    public Guid GameId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
}
