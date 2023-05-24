namespace GameTrip.Domain.Models.LocationModels;
public class ListLocationUpdateRequest
{
    public Guid LocationId { get; set; }
    public GetLocationDto? Location { get; set; }

    public IEnumerable<LocationUpdateRequestNameDto>? RequestUpdate { get; set; }
}
