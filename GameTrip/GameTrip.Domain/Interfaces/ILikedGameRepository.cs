using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface ILikedGameRepository : IGenericRepository<LikedGame>
{
    Task AddLikeAsync(Game game, GameTripUser user, decimal value);
    IEnumerable<LikedGame> GetAllByGame(Game game);
    Task<IEnumerable<LikedGame>> GetAllGameIncludeAllAsync();
    LikedGame GetLikedGame(LikedGame likedGame);
    Task RemoveLike(Game game, GameTripUser user);
}