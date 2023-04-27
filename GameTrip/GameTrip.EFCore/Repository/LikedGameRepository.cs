using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class LikedGameRepository : GenericRepository<LikedGame>, ILikedGameRepository
{
    public LikedGameRepository(GameTripContext context) : base(context)
    {
    }
}