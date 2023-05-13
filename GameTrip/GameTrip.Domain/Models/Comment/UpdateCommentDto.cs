namespace GameTrip.Domain.Models.Comment;
public class UpdateCommentDto
{
    public Guid CommentId { get; set; }
    public Guid LocationId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; }
}
