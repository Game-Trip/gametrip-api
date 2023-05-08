using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class LikedGameRepository : GenericRepository<LikedGame>, ILikedGameRepository
{
    public LikedGameRepository(GameTripContext context) : base(context)
    {
    }

    public async Task AddLikeAsync(Game game, GameTripUser user, decimal value)
    {
        LikedGame likedGame = new()
        {
            GameId = game.IdGame,
            Game = game,
            UserId = user.Id,
            User = user,
            Vote = value
        };
        await _context.LikedGame.AddAsync(likedGame);
        await _context.SaveChangesAsync();
    }
    public IEnumerable<LikedGame> GetAllByGame(Game game) => _context.LikedGame.Where(lg => lg.Game == game).AsEnumerable();
    public async Task<IEnumerable<LikedGame>> GetAllGameIncludeAllAsync() => await _context.LikedGame.Include(lg => lg.Game).ToListAsync();
    public LikedGame GetLikedGame(LikedGame likedGame) => _context.LikedGame.Include(lg => lg.Game).First(lg => lg.IdLikedGame == likedGame.IdLikedGame);

    public async Task RemoveLike(Game game, GameTripUser user)
    {
        LikedGame? likedGame = await _context.LikedGame.FirstOrDefaultAsync(ll => ll.GameId == game.IdGame && ll.UserId == user.Id);

        if (likedGame is not null)
            _context.LikedGame.Remove(likedGame!);
        await _context.SaveChangesAsync();
    }
}