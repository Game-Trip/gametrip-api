using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    void CreateLocation(Location location);
    Task<IEnumerable<Location>> GetAllLocationAsync();
    Location? GetLocationByName(string name);
    Location? GetLocationByPosition(decimal latitude, decimal longitude);
}