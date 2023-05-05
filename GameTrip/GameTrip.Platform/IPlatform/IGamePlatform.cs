using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    Task AddGameToLocationByIdAsync(Game game, Location location);
    Task CreateGameAsync(Game value);
    Task DeleteGameAsync(Game game);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName);
    IEnumerable<Game> LimitList(IEnumerable<Game> games, int limit);
    Task RemoveGameToLocationByIdAsync(Game game, Location location);
    IEnumerable<Game> SortLikedGamesByScore(IEnumerable<Game> games);
    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}