namespace GameTrip.Domain.Models.SearchModels;
public class SearchLocationDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
