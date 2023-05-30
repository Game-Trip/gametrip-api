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

    public static GameDto ToGameDto(this Game game)
    {
        return new()
        {
            Name = game.Name,
            Description = game.Description,
            Editor = game.Editor,
            ReleaseDate = game.ReleaseDate,
            Locations = game.Locations.ToEnumerable_LocationNameDto(),
            Pictures = game.Pictures.ToEnumerable_ListPictureDto(),
            LikedGames = game.LikedGames.ToEnumerable_ListLikedGameDto(),
        };
    }

    public static GameNameDto ToGameNameDto(this Game game)
    {
        return new()
        {
            Id = game.IdGame,
            Name = game.Name,
        };
    }

    public static IEnumerable<GameNameDto> ToEnumerable_GameNameDto(this IEnumerable<Game> games) => games.Select(g => g.ToGameNameDto());

    public static List<ListGameDto> ToList_ListGameDto(this IEnumerable<Game> games)
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