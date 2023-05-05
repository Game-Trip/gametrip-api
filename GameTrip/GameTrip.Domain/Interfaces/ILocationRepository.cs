using GameTrip.Domain.Entities;

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
}