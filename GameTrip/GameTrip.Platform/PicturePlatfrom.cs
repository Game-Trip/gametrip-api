using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.PictureModels;
using GameTrip.Platform.IPlatform;
using System.Runtime.InteropServices;

namespace GameTrip.Platform;
public class PicturePlatfrom : IPicturePlatfrom
{
    private readonly IUnitOfWork _unitOfWork;

    public PicturePlatfrom(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddPictureToGameAsync(AddPictureToGameDto dto, Game game, [Optional] bool force)
    {
        await _unitOfWork.Pictures.AddPictureToGameAsync(dto, game, force);
        await _unitOfWork.CompletAsync();
    }

    public async Task AddPictureToLocationAsync(AddPictureToLocationDto dto, Location location, [Optional] bool force)
    {
        await _unitOfWork.Pictures.AddPictureToLocationAsync(dto, location, force);
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
    public async Task SwitchValidateStatusPictureAsync(Picture picture)
    {
        await _unitOfWork.Pictures.SwitchValidateStatePictureAsync(picture);
        await _unitOfWork.CompletAsync();
    }
}
