using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.SearchModels;
using System.Runtime.InteropServices;

namespace GameTrip.Domain.Extension;

public static class GameExtension
{
    public static Game ToEntity(this CreateGameDto dto, [Optional] bool froce)
    {
        return new Game
        {
            Name = dto.Name,
            Description = dto.Description,
            Editor = dto.Editor,
            ReleaseDate = dto.ReleaseDate ?? null,
            AuthorId = dto.AuthorId,
            IsValidate = froce,
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
            AuthorId = game.AuthorId,
            IsValidate = game.IsValidate,
        };
    }

    public static GetGameDto ToGetGameDto(this Game game)
    {
        return new()
        {
            GameId = game.IdGame,
            Name = game.Name,
            Description = game.Description,
            Editor = game.Editor,
            ReleaseDate = game.ReleaseDate,
            Locations = game.Locations.ToList_LocationDto(),
            Pictures = game.Pictures.ToCollection_ListPictureDto(),
            LikedGames = game.LikedGames.ToCollection_ListLikedGameDto(),
            AuthorId = game.AuthorId,
            IsValidate = game.IsValidate,
        };
    }

    public static GameNameDto ToGameNameDto(this Game game)
    {
        return new()
        {
            Id = game.IdGame,
            Name = game.Name,
            AuthorId = game.AuthorId,
            IsValidate = game.IsValidate,
        };
    }
    public static SearchedGameDto ToSearchedGameDto(this Game game)
    {
        return new()
        {
            Id = game.IdGame,
            Name = game.Name,
            AuthorId = game.AuthorId,
            IsValidate = game.IsValidate,
            Locations = game.Locations.ToEnumerable_LocationDto(),
        };
    }

    public static IEnumerable<SearchedGameDto> ToEnumerable_SearchedGameDto(this IEnumerable<Game> games) => games.Select(g => g.ToSearchedGameDto());

    public static List<ListGameDto> ToList_ListGameDto(this IEnumerable<Game> games)
    {
        return games.Select(game => new ListGameDto
        {
            IdGame = game.IdGame,
            Name = game.Name,
            Description = game.Description,
            Editor = game.Editor,
            ReleaseDate = game.ReleaseDate,
            AuthorId = game.AuthorId,
            IsValidate = game.IsValidate,
        }).ToList();
    }
}