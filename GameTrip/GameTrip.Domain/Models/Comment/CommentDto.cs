using GameTrip.Domain.Models.Auth;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Models.Comment;
public class CommentDto
{
    public Guid CommentId { get; set; }
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public GameTripUserDtoName User { get; set; }
    public Guid LocationId { get; set; }
    public LocationNameDto Location { get; set; }
    public bool IsValidate { get; set; }
}
