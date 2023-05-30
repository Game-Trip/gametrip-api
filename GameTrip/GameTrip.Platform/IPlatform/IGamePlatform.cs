using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using System.Runtime.InteropServices;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    Task AddGameToLocationByIdAsync(Game game, Location location);
    Task CreateGameAsync(Game value);
    Task DeleteGameAsync(Game game);
    Task<IEnumerable<Game>> GetAllGamesAsync([Optional] int limit);
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName);
    Task RemoveGameToLocationByIdAsync(Game game, Location location);
    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}