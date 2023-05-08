using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;
public class LikePlatform : ILikePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public LikePlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddLikeToGameAsync(Game game, GameTripUser user, decimal value)
    {
        await _unitOfWork.LikedGames.AddLikeAsync(game, user, value);
        await _unitOfWork.CompletAsync();
    }

    public async Task AddLikeToLocationAsync(Location location, GameTripUser user, decimal value)
    {
        await _unitOfWork.LikedLocations.AddLikeAsync(location, user, value);
        await _unitOfWork.CompletAsync();
    }

    public IEnumerable<LikedGame> GetAllLikedGameByGame(Game game) => _unitOfWork.LikedGames.GetAllByGame(game);
    public async Task<IEnumerable<LikedGame>> GetAllLikedGameIncludeAllAsync() => await _unitOfWork.LikedGames.GetAllGameIncludeAllAsync();
    public IEnumerable<LikedLocation> GetAllLikedLocationByLocation(Location location) => _unitOfWork.LikedLocations.GetAllByLocation(location);
    public async Task<IEnumerable<LikedLocation>> GetAllLikedLocationIncludeAllAsync() => await _unitOfWork.LikedLocations.GetAllLocationIncludeAllAsync();
    public LikedGame GetLikeGame(LikedGame likedGame) => _unitOfWork.LikedGames.GetLikedGame(likedGame);
    public LikedLocation GetLikeLocation(LikedLocation likedLocation) => _unitOfWork.LikedLocations.GetLikedLocation(likedLocation);
    public async Task RemoveLikeToGameAsync(Game game, GameTripUser user)
    {
        await _unitOfWork.LikedGames.RemoveLike(game, user);
        await _unitOfWork.CompletAsync();
    }

    public async Task RemoveLikeToLocationAsync(Location location, GameTripUser user)
    {
        await _unitOfWork.LikedLocations.RemoveLike(location, user);
        await _unitOfWork.CompletAsync();
    }
}
