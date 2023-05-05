namespace GameTrip.Domain.Models.GameModels;
public class UpdateGameDto
{
    public Guid gameId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
}
