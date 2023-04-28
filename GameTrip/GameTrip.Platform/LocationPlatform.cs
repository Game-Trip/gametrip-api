using GameTrip.Domain.Entities;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;

namespace GameTrip.Platform;

public class LocationPlatform : ILocationPlarform
{
    private readonly ILocationProvider _locationProvider;

    public LocationPlatform(ILocationProvider locationProvider) => _locationProvider = locationProvider;

    public Location? GetLocationByName(string name) => _locationProvider.GetLocationByName(name);
}