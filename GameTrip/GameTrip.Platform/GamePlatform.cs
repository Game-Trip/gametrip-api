using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;

public class GamePlatform : IGamePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public GamePlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

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
    public async Task<IEnumerable<Game>> GetAllGamesAsync() => await _unitOfWork.Games.GetAllAsync();
    public async Task<Game?> GetGameByIdAsync(Guid gameId) => await _unitOfWork.Games.GetGameByIdAsync(gameId);
    public async Task<Game?> GetGameByNameAsync(string name) => await _unitOfWork.Games.GetGameByNameAsync(name);
    public async Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId) => await _unitOfWork.Games.GetGameByLocationIdAsync(locationId);
    public async Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName) => await _unitOfWork.Games.GetGameByLocationNameAsync(locationName);
}