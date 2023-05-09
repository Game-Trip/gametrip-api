﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.PictureModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Platform;
public class PicturePlatfrom : IPicturePlatfrom
{
    private readonly IUnitOfWork _unitOfWork;

    public PicturePlatfrom(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddPictureToGameAsync(IFormFile pictureData, AddPictureToGameDto dto, Game game)
    {
        await _unitOfWork.Pictures.AddPictureToGameAsync(pictureData, dto, game);
        await _unitOfWork.CompletAsync();
    }

    public async Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location)
    {
        await _unitOfWork.Pictures.AddPictureToLocationAsync(pictureData, dto, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task DeletePictureAsync(Picture picture)
    {
        _unitOfWork.Pictures.Delete(picture);
        await _unitOfWork.CompletAsync();
    }
    public async Task<Picture> GetPictureByIdAsync(Guid pictureId) => await _unitOfWork.Pictures.GetPictureByIdAsync(pictureId);
    public async Task<IEnumerable<Picture>> GetPicturesByGameIdAsync(Game game) => await _unitOfWork.Pictures.getAllByGameIdAsync(game);
    public async Task<IEnumerable<Picture>> GetPicturesByLocationIdAsync(Location location) => await _unitOfWork.Pictures.getAllByLocationIdAsync(location);
}
