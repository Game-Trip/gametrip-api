using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;
using System.Runtime.InteropServices;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    Task CreateLocationAsync(Location location);
    Task CreateUpdateRequestAsync(RequestLocationUpdate dto);
    Task DeleteLocationAsync(Location location);
    Task DeleteRequestLocationUpdateAsync(RequestLocationUpdate requestLocationUpdate);
    Task DeleteUpdateRequestAsync(Guid? requestUpdateId);
    Task<IEnumerable<Location>> GetAllLocationAsync([Optional] int limit);
    Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame);
    Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName);
    Task<Location?> GetLocationByIdAsync(Guid locationId);
    Task<Location?> GetLocationByNameAsync(string name);
    Task<Location?> GetLocationByPositionAsync(decimal latitude, decimal longitude);
    Task<Location?> GetLocationWithRequestUpdateAsync(Guid locationId);
    Task<RequestLocationUpdate?> GetRequestUpdateLocationByIdAsync(Guid requestUpdateId);
    Task SwitchValidateStatusLocationAsync(Location location);
    Task<Location> UpdateLocationAsync(Location entity, UpdateLocationDto dto);
}