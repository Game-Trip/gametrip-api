using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;

public class GamePlatform : IGamePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public GamePlatform(IUnitOfWork unitOfWork) => _unitOfWork=unitOfWork;

    public void CreateGame(Game game)
    {
        _unitOfWork.Games.Add(game);
        _unitOfWork.Complet();
    }

    public async Task<Game?> GetGameByNameAsync(string name) => await _unitOfWork.Games.GetGameByNameAsync(name);
}