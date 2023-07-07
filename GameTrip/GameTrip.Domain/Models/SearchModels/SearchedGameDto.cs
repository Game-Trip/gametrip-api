using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.SearchModels;
public class SearchedGameDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsValidate { get; set; }

    public IEnumerable<LocationDto> Locations { get; set; }
}
