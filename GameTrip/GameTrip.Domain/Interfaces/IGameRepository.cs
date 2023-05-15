﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.SearchModels;

namespace GameTrip.Domain.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameByIdAsync(Guid gameId);

    Task<IEnumerable<Game?>> GetGameByLocationIdAsync(Guid locationId);

    Task<IEnumerable<Game?>> GetGameByLocationNameAsync(string locationName);

    Task<Game?> GetGameByNameAsync(string name);
    Task<IEnumerable<Game>> SearchGameAsync(SearchGameDto dto);
    Task<Game> UpdateGameAsync(Game entity, UpdateGameDto dto);
}