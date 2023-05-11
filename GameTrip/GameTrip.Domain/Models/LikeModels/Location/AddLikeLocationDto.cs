namespace GameTrip.Domain.Models.LikeModels.Location;
public class AddLikeLocationDto
{
    public Guid? LocationId { get; set; }
    public Guid? UserId { get; set; }
    public decimal? Value { get; set; }
}
