using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;
public interface IRequestLocationUpdateRepository : IGenericRepository<RequestLocationUpdate>
{
    Task DeleteAllUpdateRequestAsync(Guid locationId);
}
