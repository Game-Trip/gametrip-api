using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.LikeModels;
public class LikedLocationDto
{
    public Guid? LikedLocationId { get; set; }
    public Guid? LocationId { get; set; }
    public LocationNameDto? Location { get; set; }
    public IEnumerable<Guid>? UsersIds { get; set; }
    public int? NbVote { get; set; }
    public decimal? MaxValue { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? AverageValue { get; set; }
}
