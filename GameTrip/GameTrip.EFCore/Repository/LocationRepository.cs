using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(GameTripContext context) : base(context)
    {
    }

    public async Task<Location?> GetLocationByIdAsync(Guid locationId) => await _context.Location.Include(l => l.Pictures)
                                                                                                 .Include(l => l.Games)
                                                                                                 .Include(l => l.Comments)
                                                                                                 .Include(l => l.LikedLocations)
                                                                                                 .FirstOrDefaultAsync(l => l.IdLocation == locationId);

    public async Task<Location?> GetLocationByNameAsync(string name) => await _context.Location.Include(l => l.Pictures)
                                                                        .Include(l => l.Games)
                                                                        .Include(l => l.Comments)
                                                                        .Include(l => l.LikedLocations)
                                                                        .FirstOrDefaultAsync(l => l.Name == name);

    public async Task<Location?> GetLocationByPosAsync(decimal latitude, decimal longitude) => await _context.Location.Include(l => l.Pictures)
                                                                                               .Include(l => l.Games)
                                                                                               .Include(l => l.Comments)
                                                                                               .Include(l => l.LikedLocations)
                                                                                               .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
}