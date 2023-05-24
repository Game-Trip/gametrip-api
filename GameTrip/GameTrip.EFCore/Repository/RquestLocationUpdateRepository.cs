using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;
public class RquestLocationUpdateRepository : GenericRepository<RequestLocationUpdate>, IRequestLocationUpdateRepository
{
    public RquestLocationUpdateRepository(GameTripContext context) : base(context)
    {
    }
}
