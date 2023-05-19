using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Platform.IPlatform;
using System.Runtime.InteropServices;

namespace GameTrip.Platform;

public class LocationPlatform : ILocationPlarform
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationPlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Location?> GetLocationByNameAsync(string name) => await _unitOfWork.Locations.GetLocationByNameAsync(name);

    public async Task<Location?> GetLocationByPositionAsync(decimal latitude, decimal longitude) => await _unitOfWork.Locations.GetLocationByPosAsync(latitude, longitude);

    public async Task CreateLocationAsync(Location location)
    {
        _unitOfWork.Locations.Add(location);
        await _unitOfWork.CompletAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(Guid locationId) => await _unitOfWork.Locations.GetLocationByIdAsync(locationId);

    public async Task DeleteLocationAsync(Location location)
    {
        _unitOfWork.Locations.Remove(location);
        await _unitOfWork.CompletAsync();
    }

    public Task<IEnumerable<Location?>> GetLocationByGameIdAsync(Guid idGame) => _unitOfWork.Locations.GetLocationByGameIdAsync(idGame);
    public Task<IEnumerable<Location?>> GetLocationByGameNameAsync(string gameName) => _unitOfWork.Locations.GetLocationByGameNameAsync(gameName);
    public async Task<Location> UpdateLocationAsync(Location entity, UpdateLocationDto dto)
    {
        Location location = await _unitOfWork.Locations.UpdateLocationAsync(entity, dto);
        await _unitOfWork.CompletAsync();
        return location;
    }

    public async Task<IEnumerable<Location>> GetAllLocationAsync([Optional] int limit) => await _unitOfWork.Locations.GetAllAsync(limit);
    public async Task SwitchValidateStatusLocationAsync(Location location)
    {
        await _unitOfWork.Locations.SwitchStateValidateLocationAsync(location);
        await _unitOfWork.CompletAsync();
    }
}