using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;
public class LikePlatform : ILikePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public LikePlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddLikeToLocationAsync(Location location, GameTripUser user, decimal value)
    {
        await _unitOfWork.LikedLocations.AddLikeAsync(location, user, value);
        await _unitOfWork.CompletAsync();
    }
    public IEnumerable<LikedLocation> GetAllLikedLocationByLocation(Location location) => _unitOfWork.LikedLocations.GetAllByLocation(location);
}
