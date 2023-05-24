using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.SearchModels;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(GameTripContext context) : base(context)
    {
    }

    public void AddGameToLocation(Game game, Location location) => location.Games!.Add(game);
    public async Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame) => await _context.Location.Include(l => l.Games)
                                                                                                              .Include(l => l.Author)
                                                                                                              .Where(l => l.Games.Any(gl => gl.IdGame == idGame))
                                                                                                              .ToListAsync();
    public async Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName) => await _context.Location.Include(l => l.Games)
                                                                                                              .Include(l => l.Author)
                                                                                                              .Where(l => l.Games.Any(gl => gl.Name == gameName))
                                                                                                              .ToListAsync();
    public async Task<Location?> GetLocationByIdAsync(Guid locationId) => await _context.Location.Include(l => l.Pictures)
                                                                                                 .Include(l => l.Games)
                                                                                                 .Include(l => l.Comments)
                                                                                                 .Include(l => l.LikedLocations)
                                                                                                 .Include(l => l.Author)
                                                                                                 .FirstOrDefaultAsync(l => l.IdLocation == locationId);

    public async Task<Location?> GetLocationByNameAsync(string name) => await _context.Location.Include(l => l.Pictures)
                                                                        .Include(l => l.Games)
                                                                        .Include(l => l.Comments)
                                                                        .Include(l => l.LikedLocations)
                                                                        .Include(l => l.Author)
                                                                        .FirstOrDefaultAsync(l => l.Name == name);

    public async Task<Location?> GetLocationByPosAsync(decimal latitude, decimal longitude) => await _context.Location.Include(l => l.Pictures)
                                                                                               .Include(l => l.Games)
                                                                                               .Include(l => l.Comments)
                                                                                               .Include(l => l.LikedLocations)
                                                                                               .Include(l => l.Author)
                                                                                               .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
    public async Task<Location?> GetLocationWithRequestUpdateAsync(Guid locationId) => await _context.Location.Include(l => l.RequestLocationUpdates).FirstOrDefaultAsync(l => l.IdLocation == locationId);
    public void RemoveGameToLocation(Game game, Location location) => location.Games!.Remove(game);
    public async Task<IEnumerable<Location>> SearchLocationAsync(SearchLocationDto dto) => await _context.Location.Include(l => l.Pictures)
                                                                                                                  .Include(l => l.Games)
                                                                                                                  .Include(l => l.Comments)
                                                                                                                  .Include(l => l.LikedLocations)
                                                                                                                  .Include(l => l.Author)
                                                                                                                  .Where(l => string.IsNullOrWhiteSpace(dto.Name) || l.Name.Contains(dto.Name))
                                                                                                                  .Where(l => string.IsNullOrWhiteSpace(dto.Description) || l.Description.Contains(dto.Description))
                                                                                                                  .Where(l => dto.Latitude == null || l.Latitude == dto.Latitude)
                                                                                                                  .Where(l => dto.Longitude == null || l.Longitude == dto.Longitude)
                                                                                                                  .ToListAsync();
    public async Task SwitchStateValidateLocationAsync(Location location)
    {
        location.IsValid = !location.IsValid;
        await _context.SaveChangesAsync();
    }

    public async Task<Location> UpdateLocationAsync(Location entity, UpdateLocationDto dto)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;

        await _context.SaveChangesAsync();
        return entity;
    }
}