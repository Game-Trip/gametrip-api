using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Extension;
public static class GameUpdateRequestExtension
{
    public static RequestGameUpdate ToEntity(this GameUpdateRequestDto dto)
    {
        return new()
        {
            GameId = dto.GameId,
            Name = dto.Name ?? null,
            Description = dto.Description ?? null,
            Editor = dto.Editor ?? null,
            ReleaseDate = dto.ReleaseDate ?? null,
        };
    }

    public static GameUpdateRequestNameDto ToGameUpdateRequestNameDto(this RequestGameUpdate requestGameUpdate)
    {
        return new()
        {
            IdRequestUpdateGame = requestGameUpdate.IdRequestGameUpdate,
            NewName = requestGameUpdate.Name ?? null,
            NewDescription = requestGameUpdate.Description ?? null,
            NewEditor = requestGameUpdate.Editor ?? null,
            NewReleaseDate = requestGameUpdate.ReleaseDate ?? null,
            Picture = requestGameUpdate.Picture?.ToPictureDto(),
            IsAddedPicture = requestGameUpdate.isAddedPicture ?? null,
            Location = requestGameUpdate.Location?.ToLocationNameDto(),
            IsAddedLocation = requestGameUpdate.isAddedLocation ?? null
        };
    }

    public static IEnumerable<GameUpdateRequestNameDto> ToEnumerable_GameUpdateRequestNameDto(this ICollection<RequestGameUpdate> requestGameUpdates) => requestGameUpdates.Select(rg => rg.ToGameUpdateRequestNameDto());

    public static ListGameUpdateRequest ToListGameUpdateRequest(this Game game)
    {
        return new()
        {
            GameId = game.IdGame,
            Game = game.ToGetGameDto() ?? null,
            RequestUpdate = game.RequestGameUpdates.ToEnumerable_GameUpdateRequestNameDto() ?? null
        };
    }
}
