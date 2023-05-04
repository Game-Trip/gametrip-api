using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    Task CreateGameAsync(Game value);
    Task DeleteGameAsync(Game game);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName);
}