using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;
public interface ILikePlatform
{
    Task AddLikeToGameAsync(Game game, GameTripUser user, decimal value);
    Task AddLikeToLocationAsync(Location location, GameTripUser user, decimal value);
    IEnumerable<LikedGame> GetAllLikedGameByGame(Game game);
    Task<IEnumerable<LikedGame>> GetAllLikedGameIncludeAllAsync();
    IEnumerable<LikedLocation> GetAllLikedLocationByLocation(Location location);
    Task<IEnumerable<LikedLocation>> GetAllLikedLocationIncludeAllAsync();
    LikedGame GetLikeGame(LikedGame likedGame);
    LikedLocation GetLikeLocation(LikedLocation likedLocation);
    Task RemoveLikeToGameAsync(Game game, GameTripUser user);
    Task RemoveLikeToLocationAsync(Location location, GameTripUser user);
}
