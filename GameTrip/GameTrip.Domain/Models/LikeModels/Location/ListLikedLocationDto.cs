using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.LikeModels.Location;
public class ListLikedLocationDto
{
    public Guid? LikedLocationId { get; set; }
    public Guid? LocationId { get; set; }
    public LocationNameDto? Location { get; set; }
}
