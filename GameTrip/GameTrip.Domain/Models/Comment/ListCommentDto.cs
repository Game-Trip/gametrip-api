namespace GameTrip.Domain.Models.Comment;
public class ListCommentDto
{
    public Guid CommentId { get; set; }
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public Guid LocationId { get; set; }
    public bool IsValidate { get; set; }
}
