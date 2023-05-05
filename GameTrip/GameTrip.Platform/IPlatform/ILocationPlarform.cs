using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    Task CreateLocationAsync(Location location);
    Task DeleteLocationAsync(Location location);
    Task<IEnumerable<Location>> GetAllLocationAsync();
    Task<Location?> GetLocationByIdAsync(Guid locationId);
    Task<Location?> GetLocationByNameAsync(string name);
    Task<Location?> GetLocationByPositionAsync(decimal latitude, decimal longitude);
}