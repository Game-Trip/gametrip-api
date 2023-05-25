using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Platform.IPlatform;
using System.Runtime.InteropServices;

namespace GameTrip.Platform;

public class GamePlatform : IGamePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public GamePlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddGameToLocationByIdAsync(Game game, Location location)
    {
        _unitOfWork.Locations.AddGameToLocation(game, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task RemoveGameToLocationByIdAsync(Game game, Location location)
    {
        _unitOfWork.Locations.RemoveGameToLocation(game, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task CreateGameAsync(Game game)
    {
        _unitOfWork.Games.Add(game);
        await _unitOfWork.CompletAsync();
    }

    public async Task DeleteGameAsync(Game game)
    {
        _unitOfWork.Games.Remove(game);
        await _unitOfWork.CompletAsync();
    }

    public async Task<IEnumerable<Game>> GetAllGamesAsync([Optional] int limit) => await _unitOfWork.Games.GetAllAsync(limit);

    public async Task<Game?> GetGameByIdAsync(Guid gameId) => await _unitOfWork.Games.GetGameByIdAsync(gameId);

    public async Task<Game?> GetGameByNameAsync(string name) => await _unitOfWork.Games.GetGameByNameAsync(name);

    public async Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId) => await _unitOfWork.Games.GetGameByLocationIdAsync(locationId);

    public async Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName) => await _unitOfWork.Games.GetGameByLocationNameAsync(locationName);

    public async Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto)
    {
        Game game = await _unitOfWork.Games.UpdateGameAsync(entity, dto);
        await _unitOfWork.CompletAsync();
        return game;
    }

    public async Task SwitchValidateStatusGameAsync(Game game)
    {
        await _unitOfWork.Games.SwitchValidateStateGameAsync(game);
        await _unitOfWork.CompletAsync();
    }

    public async Task DeleteUpdateGameRequestAsync(Guid gameId)
    {
        await _unitOfWork.RequestGameUpdate.DeleteAllUpdateRequestAsync(gameId);
        await _unitOfWork.CompletAsync();
    }

    public async Task CreateUpdateRequestAsync(RequestGameUpdate requestGameUpdate)
    {
        _unitOfWork.RequestGameUpdate.Add(requestGameUpdate);
        await _unitOfWork.CompletAsync();
    }

    public async Task<Game?> GetGameWithRequestUpdateAsync(Guid gameId) => await _unitOfWork.Games.GetGameWithRequestUpdateAsync(gameId);
}