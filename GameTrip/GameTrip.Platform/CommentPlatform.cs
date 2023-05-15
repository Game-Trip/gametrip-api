using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.Comment;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;
public class CommentPlatform : ICommentPlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentPlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddCommentToLocationAsync(Location location, GameTripUser user, AddCommentToLocationDto dto)
    {
        await _unitOfWork.Comments.AddCommentTolocationAsync(location, user, dto);
        await _unitOfWork.CompletAsync();
    }

    public async Task DeleteAsync(Comment comment)
    {
        _unitOfWork.Comments.Remove(comment);
        await _unitOfWork.CompletAsync();
    }

    public IEnumerable<Comment>? GetCommentAllByLocationId(Guid locationId) => _unitOfWork.Comments.GetAllCommentsByLocationId(locationId);
    public IEnumerable<Comment>? GetCommentAllByUserId(Guid id) => _unitOfWork.Comments.GetAllCommentsByUserId(id);
    public async Task<Comment?> GetCommentByIdAsync(Guid commentId) => await _unitOfWork.Comments.GetCommentByIdAsync(commentId);
    public async Task UpdateCommentAsync(Comment entity, UpdateCommentDto dto)
    {
        await _unitOfWork.Comments.UpdateCommentAsync(entity, dto);
        await _unitOfWork.CompletAsync();
    }
}
