using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using System.Runtime.InteropServices;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    Task AddGameToLocationByIdAsync(Game game, Location location);
    Task CreateGameAsync(Game value);
    Task CreateUpdateRequestAsync(RequestGameUpdate requestGameUpdate);
    Task DeleteGameAsync(Game game);
    Task DeleteUpdateGameRequestAsync(Guid gameId);
    Task<IEnumerable<Game>> GetAllGamesAsync([Optional] int limit);
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName);
    Task<Game?> GetGameWithRequestUpdateAsync(Guid gameId);
    Task RemoveGameToLocationByIdAsync(Game game, Location location);
    Task SwitchValidateStatusGameAsync(Game game);
    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}