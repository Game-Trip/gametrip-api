using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.PictureModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Platform;
public class PicturePlatfrom : IPicturePlatfrom
{
    private readonly IUnitOfWork _unitOfWork;

    public PicturePlatfrom(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location)
    {
        await _unitOfWork.Pictures.AddPictureToLocationAsync(pictureData, dto, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task<IEnumerable<Picture>> GetPicturesByLocationIdAsync(Location location) => await _unitOfWork.Pictures.getAllByLocationIdAsync(location);
}
