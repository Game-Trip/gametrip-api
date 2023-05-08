using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    public async Task RemoveLike(Location location, GameTripUser user)
    {
        LikedLocation? likedLocation = await _context.LikedLocation.FirstOrDefaultAsync(ll => ll.LocationId == location.IdLocation && ll.UserId == user.Id);

        if (likedLocation is not null)
            _context.LikedLocation.Remove(likedLocation!);
        await _context.SaveChangesAsync();
    }
}