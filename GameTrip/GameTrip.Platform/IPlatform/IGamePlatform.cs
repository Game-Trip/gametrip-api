using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    void CreateGame(Game value);

    Task<Game?> GetGameByNameAsync(string name);
}