using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.AspNetCore.Http;

namespace GameTrip.Domain.Interfaces;

public interface IPictureRepository : IGenericRepository<Picture>
{
    Task AddPictureToGameAsync(IFormFile pictureData, AddPictureToGameDto dto, Game game);
    Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location);
    void Delete(Picture picture);
    Task<IEnumerable<Picture>> getAllByGameIdAsync(Game game);
    Task<IEnumerable<Picture>> getAllByLocationIdAsync(Location location);
    Task<Picture> GetPictureByIdAsync(Guid pictureId);
}