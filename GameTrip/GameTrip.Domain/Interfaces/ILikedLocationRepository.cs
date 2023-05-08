using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface ILikedLocationRepository : IGenericRepository<LikedLocation>
{
    Task AddLikeAsync(Location location, GameTripUser user, decimal value);
    IEnumerable<LikedLocation> GetAllByLocation(Location location);
}