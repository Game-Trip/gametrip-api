using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.Comment;

namespace GameTrip.Domain.Extension;
public static class CommentExtension
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new()
        {
            CommentId = comment.IdComment,
            Text = comment.Text,
            UserId = comment.UserId,
            User = comment.User?.ToGameTripUserDtoName(),
            LocationId = comment.LocationId,
            Location = comment.Location?.ToLocationNameDto(),
            IsValidate = comment.IsValidate
        };
    }

    public static IEnumerable<ListCommentDto> ToEnumerable_ListCommentDto(this IEnumerable<Comment> comments)
    {
        return comments.Select(c => new ListCommentDto()
        {
            CommentId = c.IdComment,
            Text = c.Text,
            UserId = c.UserId,
            LocationId = c.LocationId,
            IsValidate = c.IsValidate,
        });
    }
}
