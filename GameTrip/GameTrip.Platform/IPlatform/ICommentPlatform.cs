using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.Comment;

namespace GameTrip.Platform.IPlatform;
public interface ICommentPlatform
{
    Task AddCommentToLocationAsync(Location location, GameTripUser user, AddCommentToLocationDto dto);
    Task DeleteAsync(Comment comment);
    IEnumerable<Comment>? GetCommentAllByLocationId(Guid locationId);
    IEnumerable<Comment>? GetCommentAllByUserId(Guid id);
    Task<Comment?> GetCommentByIdAsync(Guid commentId);
    Task UpdateCommentAsync(Comment entity, UpdateCommentDto dto);
}
