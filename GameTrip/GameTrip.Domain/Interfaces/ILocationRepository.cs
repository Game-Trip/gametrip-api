using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.SearchModels;

namespace GameTrip.Domain.Interfaces;

public interface ILocationRepository : IGenericRepository<Location>
{
    void AddGameToLocation(Game game, Location location);
    Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame);
    Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName);
    Task<Location?> GetLocationByIdAsync(Guid locationId);

    Task<Location?> GetLocationByNameAsync(string name);

    Task<Location?> GetLocationByPosAsync(decimal latitude, decimal longitude);
    void RemoveGameToLocation(Game game, Location location);
    Task<IEnumerable<Location>> SearchLocationAsync(SearchLocationDto dto);
    Task SwitchStateValidateLocationAsync(Location location);
    Task<Location> UpdateLocationAsync(Location entity, UpdateLocationDto dto);
}