using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    void CreateLocation(Location location);
    Task DeleteLocation(Location location);
    Task<IEnumerable<Location>> GetAllLocationAsync();
    Task<Location?> GetLocationByIdAsync(Guid locationId);
    Task<Location?> GetLocationByNameAsync(string name);
    Task<Location?> GetLocationByPositionAsync(decimal latitude, decimal longitude);
}