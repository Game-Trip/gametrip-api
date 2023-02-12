using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class LikedLocationRepository : GenericRepository<LikedLocation>, ILikedLocationRepository
{
    public LikedLocationRepository(GameTripContext context) : base(context)
    {

    }
}
