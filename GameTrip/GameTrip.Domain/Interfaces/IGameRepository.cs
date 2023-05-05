using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<IEnumerable<Game?>> GetGameByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGameByLocationNameAsync(string locationName);
    Task<Game?> GetGameByNameAsync(string name);
}