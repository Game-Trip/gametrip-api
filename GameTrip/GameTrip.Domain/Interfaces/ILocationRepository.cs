using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface ILocationRepository : IGenericRepository<Location>
{
    Task<Location?> GetLocationByIdAsync(Guid locationId);
    Task<Location?> GetLocationByNameAsync(string name);
    Task<Location?> GetLocationByPosAsync(decimal latitude, decimal longitude);
}