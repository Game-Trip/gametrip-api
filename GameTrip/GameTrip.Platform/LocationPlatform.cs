﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Platform.IPlatform;

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

    public async Task<IEnumerable<Location>> GetAllLocationAsync() => await _unitOfWork.Locations.GetAllAsync();

    public async Task<Location?> GetLocationByIdAsync(Guid locationId) => await _unitOfWork.Locations.GetLocationByIdAsync(locationId);

    public async Task DeleteLocationAsync(Location location)
    {
        _unitOfWork.Locations.Remove(location);
        await _unitOfWork.CompletAsync();
    }
}