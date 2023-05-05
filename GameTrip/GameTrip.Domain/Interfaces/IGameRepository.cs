using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameByIdAsync(Guid gameId);

    Task<IEnumerable<Game?>> GetGameByLocationIdAsync(Guid locationId);

    Task<IEnumerable<Game?>> GetGameByLocationNameAsync(string locationName);

    Task<Game?> GetGameByNameAsync(string name);

    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}