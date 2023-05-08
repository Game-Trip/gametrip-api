using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Platform.IPlatform;
public interface IPicturePlatfrom
{
    Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location);
    Task<IEnumerable<Picture>> GetPicturesByLocationIdAsync(Location location);
}
