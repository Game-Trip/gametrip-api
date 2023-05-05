using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(GameTripContext context) : base(context)
    {
    }

    public void AddGameToLocation(Game game, Location location) => location.Games!.Add(game);
    public async Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame) => await _context.Location.Include(l => l.Games)
                                                                                                              .Where(l => l.Games.Any(gl => gl.IdGame == idGame))
                                                                                                              .ToListAsync();
    public async Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName) => await _context.Location.Include(l => l.Games)
                                                                                                              .Where(l => l.Games.Any(gl => gl.Name == gameName))
                                                                                                              .ToListAsync();
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
    public void RemoveGameToLocation(Game game, Location location) => location.Games!.Remove(game);

}