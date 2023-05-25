using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;
public class RequestGameUpdateRepository : GenericRepository<RequestGameUpdate>, IRequestGameUpdateRepository
{
    public RequestGameUpdateRepository(GameTripContext context) : base(context)
    {

    }
}
