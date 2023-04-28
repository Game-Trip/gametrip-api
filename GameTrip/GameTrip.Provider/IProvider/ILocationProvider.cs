using GameTrip.Domain.Entities;

namespace GameTrip.Provider.IProvider;

public interface ILocationProvider
{
    Location? GetLocationByName(string name);
}