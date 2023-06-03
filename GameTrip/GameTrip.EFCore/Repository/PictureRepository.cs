using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.PictureModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace GameTrip.EFCore.Repository;

public class PictureRepository : GenericRepository<Picture>, IPictureRepository
{
    public PictureRepository(GameTripContext context) : base(context)
    {
    }

    public async Task AddPictureToGameAsync(AddPictureToGameDto dto, Game game, [Optional] bool force)
    {
        Picture picture = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            GameId = game.IdGame,
            Game = game,
            Data = dto.pictureData,
            AuthorId = dto.AuthorId,
            IsValidate = force,
        };
        await _context.Picture.AddAsync(picture);
        await _context.SaveChangesAsync();
    }

    public async Task AddPictureToLocationAsync(AddPictureToLocationDto dto, Location location, [Optional] bool force)
    {
        Picture picture = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            LocationId = location.IdLocation,
            Location = location,
            Data = dto.pictureData,
            AuthorId = dto.AuthorId,
            IsValidate = force
        };
        await _context.Picture.AddAsync(picture);
        await _context.SaveChangesAsync();
    }

    public void Delete(Picture picture) => _context.Picture.Remove(picture);
    public async Task<IEnumerable<Picture>> getAllByGameIdAsync(Game game) => await _context.Picture.Include(p => p.Game).Include(p => p.Author).Where(p => p.Game == game).ToListAsync();
    public async Task<Picture> GetPictureByIdAsync(Guid pictureId) => await _context.Picture.Include(p => p.Game).Include(p => p.Location).Include(p => p.Author).FirstOrDefaultAsync(p => p.IdPicture == pictureId);
    public async Task SwitchValidateStatePictureAsync(Picture picture)
    {
        picture.IsValidate = !picture.IsValidate;
        await _context.SaveChangesAsync();
    }
    async Task<IEnumerable<Picture>> IPictureRepository.getAllByLocationIdAsync(Location location) => await _context.Picture.Include(p => p.Location).Where(p => p.Location == location).ToListAsync();
}