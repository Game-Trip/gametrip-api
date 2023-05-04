using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(GameTripContext context) : base(context)
    {
    }

    public async Task<Game?> GetGameByNameAsync(string name) => await _context.Game.Include(g => g.Locations)
                                                                           .Include(g => g.Pictures)
                                                                           .Include(g => g.LikedGames)
                                                                           .FirstOrDefaultAsync(g => g.Name == name);
}