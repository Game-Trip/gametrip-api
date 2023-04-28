using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;

public interface ILocationPlarform
{
    Location? GetLocationByName(string name);
}