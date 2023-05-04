namespace GameTrip.Domain.Models.GameModels;

public class CreateGameDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? RealaseDate { get; set; }
}