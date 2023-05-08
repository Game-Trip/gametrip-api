﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Domain.Interfaces;

public interface IPictureRepository : IGenericRepository<Picture>
{
    Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location);
    Task<IEnumerable<Picture>> getAllByLocationIdAsync(Location location);
}