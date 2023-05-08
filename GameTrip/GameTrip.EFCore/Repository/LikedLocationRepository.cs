using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class LikedLocationRepository : GenericRepository<LikedLocation>, ILikedLocationRepository
{
    public LikedLocationRepository(GameTripContext context) : base(context)
    {
    }

    public async Task AddLikeAsync(Location location, GameTripUser user, decimal value)
    {
        LikedLocation likedLocation = new()
        {
            LocationId = location.IdLocation,
            Location = location,
            UserId = user.Id,
            User = user,
            Vote = value
        };
        await _context.LikedLocation.AddAsync(likedLocation);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<LikedLocation> GetAllByLocation(Location location) => _context.LikedLocation.Where(ll => ll.Location == location).AsEnumerable();
}