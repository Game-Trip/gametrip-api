namespace GameTrip.Domain.Models.SearchModels;
public class SearchGameDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
}
