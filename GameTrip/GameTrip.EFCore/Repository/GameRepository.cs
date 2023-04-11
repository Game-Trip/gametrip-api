using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(GameTripContext context) : base(context)
    {
    }
}