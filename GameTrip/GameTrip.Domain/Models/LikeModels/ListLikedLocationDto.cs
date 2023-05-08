using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.LikeModels;
public class ListLikedLocationDto
{
    public Guid? LikedLocationId { get; set; }
    public Guid? LocationId { get; set; }
    public LocationNameDto? Location { get; set; }
}
