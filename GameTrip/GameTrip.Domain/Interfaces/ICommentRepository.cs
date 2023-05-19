using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.Comment;

namespace GameTrip.Domain.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task AddCommentTolocationAsync(Location location, GameTripUser user, AddCommentToLocationDto dto, bool force);
    IEnumerable<Comment>? GetAllCommentsByLocationId(Guid locationId);
    IEnumerable<Comment>? GetAllCommentsByUserId(Guid id);
    Task<Comment?> GetCommentByIdAsync(Guid commentId);
    Task UpdateCommentAsync(Comment entity, UpdateCommentDto dto);
}