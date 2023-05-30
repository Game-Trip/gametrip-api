using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class PictureRepository : GenericRepository<Picture>, IPictureRepository
{
    public PictureRepository(GameTripContext context) : base(context)
    {
    }

    public async Task AddPictureToGameAsync(IFormFile pictureData, AddPictureToGameDto dto, Game game)
    {
        using MemoryStream stream = new();
        await pictureData.CopyToAsync(stream);
        Picture picture = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            GameId = game.IdGame,
            Game = game,
            Data = stream.ToArray()
        };
        await _context.Picture.AddAsync(picture);
        await _context.SaveChangesAsync();
    }

    public async Task AddPictureToLocationAsync(IFormFile pictureData, AddPictureToLocationDto dto, Location location)
    {
        using MemoryStream stream = new();
        await pictureData.CopyToAsync(stream);
        Picture picture = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            LocationId = location.IdLocation,
            Location = location,
            Data = stream.ToArray()
        };
        await _context.Picture.AddAsync(picture);
        await _context.SaveChangesAsync();
    }

    public void Delete(Picture picture) => _context.Picture.Remove(picture);
    public async Task<IEnumerable<Picture>> getAllByGameIdAsync(Game game) => await _context.Picture.Include(p => p.Game).Where(p => p.Game == game).ToListAsync();
    public async Task<Picture> GetPictureByIdAsync(Guid pictureId) => await _context.Picture.Include(p => p.Game).Include(p => p.Location).FirstOrDefaultAsync(p => p.IdPicture == pictureId);
    async Task<IEnumerable<Picture>> IPictureRepository.getAllByLocationIdAsync(Location location) => await _context.Picture.Include(p => p.Location).Where(p => p.Location == location).ToListAsync();
}