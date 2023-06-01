using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Interfaces;

public interface IPictureRepository : IGenericRepository<Picture>
{
    Task AddPictureToGameAsync(AddPictureToGameDto dto, Game game, bool force);
    Task AddPictureToLocationAsync(AddPictureToLocationDto dto, Location location, bool force);
    void Delete(Picture picture);
    Task<IEnumerable<Picture>> getAllByGameIdAsync(Game game);
    Task<IEnumerable<Picture>> getAllByLocationIdAsync(Location location);
    Task<Picture> GetPictureByIdAsync(Guid pictureId);
    Task SwitchValidateStatePictureAsync(Picture picture);
}