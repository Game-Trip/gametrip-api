using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Platform.IPlatform;
public interface IPicturePlatfrom
{
    Task AddPictureToGameAsync(IFormFile pictureData, AddPictureToGameDto dto, Game game, bool force);
    Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location, bool force);
    Task DeletePictureAsync(Picture picture);
    Task<Picture?> GetPictureByIdAsync(Guid pictureId);
    Task<IEnumerable<Picture>> GetPicturesByGameIdAsync(Game game);
    Task<IEnumerable<Picture>> GetPicturesByLocationIdAsync(Location location);
    Task SwitchValidateStatusPictureAsync(Picture picture);
}
