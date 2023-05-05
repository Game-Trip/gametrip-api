using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    Task CreateLocationAsync(Location location);
    Task DeleteLocationAsync(Location location);
    Task<IEnumerable<Location>> GetAllLocationAsync();
    Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame);
    Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName);
    Task<Location?> GetLocationByIdAsync(Guid locationId);
    Task<Location?> GetLocationByNameAsync(string name);
    Task<Location?> GetLocationByPositionAsync(decimal latitude, decimal longitude);
    Task<Location> UpdateLocationAsync(Location entity, UpdateLocationDto dto);
}