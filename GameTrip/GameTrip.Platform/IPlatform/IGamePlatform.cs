using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using System.Runtime.InteropServices;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    void AddGamesToLocationByIdAsync(IEnumerable<Game> games, Location? location);
    Task AddGameToLocationByIdAsync(Game game, Location location);
    Task CreateGameAsync(Game value);
    Task CreateUpdateRequestAsync(RequestGameUpdate requestGameUpdate);
    Task DeleteGameAsync(Game game);
    Task DeleteRequestGameUpdateAsync(RequestGameUpdate requestGameUpdate);
    Task DeleteUpdateGameRequestAsync(Guid? requestUpdateId);
    Task<IEnumerable<Game>> GetAllGamesAsync([Optional] int limit);
    Task<Game?> GetGameByIdAsync(Guid gameId);
    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game>> GetGameRangeByIdAsync(IEnumerable<Guid> gamesIds);
    Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId);
    Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName);
    Task<Game?> GetGameWithRequestUpdateAsync(Guid gameId);
    Task<RequestGameUpdate?> GetRequestUpdateGameByIdAsync(Guid requestUpdateId);
    Task RemoveGameToLocationByIdAsync(Game game, Location location);
    Task RequestToAddOrRemoveGameToLocationByIdAsync(RequestLocationUpdate requestLocationUpdate);
    Task SwitchValidateStatusGameAsync(Game game);
    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}