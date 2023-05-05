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

    public static GameDto ToDto(this Game game)
    {
        return new()
        {
            Name = game.Name,
            Description = game.Description,
            Editor = game.Editor,
            ReleaseDate = game.ReleaseDate,
            Locations = game.Locations.ToDtoList(),
            Pictures = game.Pictures,
            LikedGames = game.LikedGames,
        };
    }

    public static List<ListGameDto> ToDtoList(this IEnumerable<Game> games)
    {
        return games.Select(game => new ListGameDto
        {
            IdGame = game.IdGame,
            Name = game.Name,
            Description = game.Description,
            Editor = game.Editor,
            ReleaseDate = game.ReleaseDate,
        }).ToList();
    }
}