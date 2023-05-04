using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface IGamePlatform
{
    void CreateGameAsync(Game value);

    Task<Game?> GetGameByNameAsync(string name);
}