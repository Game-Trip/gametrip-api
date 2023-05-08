using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LikeModels.Game;

namespace GameTrip.Domain.Extension;
public static class LikeGameExtension
{
    public static LikedGameDto ToDto(this IEnumerable<LikedGame> likedGames)
    {
        return new()
        {
            LikedGameId = likedGames.First().IdLikedGame,
            GameId = likedGames.First().GameId,
            UsersIds = likedGames.Select(lg => lg.UserId),
            NbVote = likedGames.Count(),
            MaxValue = likedGames.OrderBy(lg => lg.Vote).First().Vote,
            MinValue = likedGames.OrderBy(lg => lg.Vote).Last().Vote,
            AverageValue = likedGames.Average(lg => lg.Vote)
        };
    }

    public static IEnumerable<ListLikedGameDto> ToListLikedGameDto(this IEnumerable<LikedGame> likedGames)
    {
        return likedGames.Select(lg => new ListLikedGameDto()
        {
            LikedGameId = lg.IdLikedGame,
            GameId = lg.GameId,
            Game = lg.Game.ToGameNameDto(),
        }).AsEnumerable();
    }
}
