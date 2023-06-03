using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Platform.IPlatform;
public interface IPicturePlatfrom
{
    Task AddPictureToGameAsync(AddPictureToGameDto dto, Game game, bool force);
    Task AddPictureToLocationAsync(AddPictureToLocationDto dto, Location location, bool force);
    Task DeletePictureAsync(Picture picture);
    Task<Picture?> GetPictureByIdAsync(Guid pictureId);
    Task<IEnumerable<Picture>> GetPicturesByGameIdAsync(Game game);
    Task<IEnumerable<Picture>> GetPicturesByLocationIdAsync(Location location);
    Task SwitchValidateStatusPictureAsync(Picture picture);
}
