using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(GameTripContext context) : base(context)
    {

    }
}
