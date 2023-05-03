using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(GameTripContext context) : base(context)
    {
    }

    public Location? GetLocationByName(string name) => _context.Location.FirstOrDefault(l => l.Name == name);
    public Location? GetLocationByPos(decimal latitude, decimal longitude) => _context.Location.FirstOrDefault(l => l.Latitude == latitude && l.Longitude == longitude);
}