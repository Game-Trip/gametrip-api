using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.PictureModels;
public class ListPictureLocationDto
{
    public Guid LocationId { get; set; }
    public LocationNameDto? Location { get; set; }
    public IEnumerable<PictureDto>? Pictures { get; set; }
}
