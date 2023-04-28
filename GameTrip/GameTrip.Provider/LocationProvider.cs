using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Provider.IProvider;

namespace GameTrip.Provider;

public class LocationProvider : ILocationProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Location? GetLocationByName(string name) => (Location?)_unitOfWork.Locations.Find(l => l.Name == name);
}