using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;

public class GamePlatform : IGamePlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public GamePlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task AddGameToLocationByIdAsync(Game game, Location location)
    {
        _unitOfWork.Locations.AddGameToLocation(game, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task RemoveGameToLocationByIdAsync(Game game, Location location)
    {
        _unitOfWork.Locations.RemoveGameToLocation(game, location);
        await _unitOfWork.CompletAsync();
    }

    public async Task CreateGameAsync(Game game)
    {
        _unitOfWork.Games.Add(game);
        await _unitOfWork.CompletAsync();
    }

    public async Task DeleteGameAsync(Game game)
    {
        _unitOfWork.Games.Remove(game);
        await _unitOfWork.CompletAsync();
    }

    public async Task<IEnumerable<Game>> GetAllGamesAsync() => await _unitOfWork.Games.GetAllAsync();

    public async Task<Game?> GetGameByIdAsync(Guid gameId) => await _unitOfWork.Games.GetGameByIdAsync(gameId);

    public async Task<Game?> GetGameByNameAsync(string name) => await _unitOfWork.Games.GetGameByNameAsync(name);

    public async Task<IEnumerable<Game?>> GetGamesByLocationIdAsync(Guid locationId) => await _unitOfWork.Games.GetGameByLocationIdAsync(locationId);

    public async Task<IEnumerable<Game?>> GetGamesByLocationNameAsync(string locationName) => await _unitOfWork.Games.GetGameByLocationNameAsync(locationName);

    public IEnumerable<Game> SortLikedGamesByScore(IEnumerable<Game> games)
    {
        return games.OrderBy(g =>
        {
            decimal score = 0.0m;
            int nbVote = 0;
            g.LikedGames!.ToList().ForEach(l =>
            {
                nbVote++;
                score += l.vote;
            });
            return score/nbVote;
        });
    }

    public IEnumerable<Game> LimitList(IEnumerable<Game> games, int limit) => games.Take(limit);

    public async Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto)
    {
        Game game = await _unitOfWork.Games.UpdateGameAsync(entity, dto);
        await _unitOfWork.CompletAsync();
        return game;
    }
}