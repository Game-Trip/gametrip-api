using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Extension;
public static class GameExtension
{
    public static Game ToEntity(this CreateGameDto dto)
    {
        return new Game
        {
            Name = dto.Name,
            Description = dto.Description,
            Editor = dto.Editor,
            ReleaseDate = dto.ReleaseDate ?? null,
        };
    }
}
