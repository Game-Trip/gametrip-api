using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.SearchModels;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(GameTripContext context) : base(context)
    {
    }

    public async Task<Game?> GetGameByIdAsync(Guid gameId) => await _context.Game.Include(g => g.Locations)
                                                                                 .Include(g => g.Pictures)
                                                                                 .Include(g => g.LikedGames)
                                                                                 .FirstOrDefaultAsync(g => g.IdGame == gameId);

    public async Task<Game?> GetGameByNameAsync(string name) => await _context.Game.Include(g => g.Locations)
                                                                       .Include(g => g.Pictures)
                                                                       .Include(g => g.LikedGames)
                                                                       .FirstOrDefaultAsync(g => g.Name == name);

    public async Task<IEnumerable<Game?>> GetGameByLocationIdAsync(Guid locationId) => await _context.Game.Include(g => g.Locations)
                                                                                                          .Where(g => g.Locations.Any(gl => gl.IdLocation == locationId))
                                                                                                          .ToListAsync();

    public async Task<IEnumerable<Game?>> GetGameByLocationNameAsync(string locationName) => await _context.Game.Include(g => g.Locations)
                                                                                                                .Where(g => g.Locations.Any(gl => gl.Name == locationName))
                                                                                                                .ToListAsync();

    public async Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Editor = dto.Editor;
        entity.ReleaseDate = dto.ReleaseDate;

        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Game>> SearchGameAsync(SearchGameDto dto) => await _context.Game.Include(g => g.Locations)
                                                                                                  .Include(g => g.Pictures)
                                                                                                  .Include(g => g.LikedGames)
                                                                                                  .Where(g => string.IsNullOrEmpty(dto.Name) || g.Name.Contains(dto.Name))
                                                                                                  .Where(g => string.IsNullOrEmpty(dto.Description) || g.Description.Contains(dto.Description))
                                                                                                  .Where(g => string.IsNullOrEmpty(dto.Editor) || g.Editor.Contains(dto.Editor))
                                                                                                  .Where(g => dto.ReleaseDate == null || g.ReleaseDate == dto.ReleaseDate)
                                                                                                  .ToListAsync();
}