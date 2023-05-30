using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.Comment;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(GameTripContext context) : base(context)
    {
    }

    public async Task AddCommentTolocationAsync(Location location, GameTripUser user, AddCommentToLocationDto dto)
    {
        await _context.Comment.AddAsync(new Comment()
        {
            Location = location,
            LocationId = dto.LocationId,

            User = user,
            UserId = dto.UserId,

            Text = dto.Text,
        });
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Comment>? GetAllCommentsByLocationId(Guid locationId) => _context.Comment.Where(c => c.LocationId == locationId).AsEnumerable();
    public IEnumerable<Comment>? GetAllCommentsByUserId(Guid id) => _context.Comment.Where(c => c.UserId == id).AsEnumerable();
    public async Task<Comment?> GetCommentByIdAsync(Guid commentId) => await _context.Comment.Include(c => c.User).Include(c => c.Location).FirstOrDefaultAsync(c => c.IdComment == commentId);
    public async Task UpdateCommentAsync(Comment entity, UpdateCommentDto dto)
    {
        entity.Text = dto.Text;
        await _context.SaveChangesAsync();
    }
}