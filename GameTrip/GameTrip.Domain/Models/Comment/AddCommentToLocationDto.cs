namespace GameTrip.Domain.Models.Comment;
public class AddCommentToLocationDto
{
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public Guid LocationId { get; set; }
}
