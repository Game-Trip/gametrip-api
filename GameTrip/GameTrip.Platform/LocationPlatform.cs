using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;

public class LocationPlatform : ILocationPlarform
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationPlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Location? GetLocationByName(string name) => _unitOfWork.Locations.GetLocationByName(name);
    public Location? GetLocationByPosition(decimal latitude, decimal longitude) => _unitOfWork.Locations.GetLocationByPos(latitude, longitude);
    public void CreateLocation(Location location)
    {
        _unitOfWork.Locations.Add(location);
        _unitOfWork.Complet();
    }

    public async Task<IEnumerable<Location>> GetAllLocationAsync() => await _unitOfWork.Locations.GetAllAsync();
}