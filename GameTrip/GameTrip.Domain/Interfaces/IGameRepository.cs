using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameByNameAsync(string name);
}