using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(GameTripContext context) : base(context)
    {

    }
}
