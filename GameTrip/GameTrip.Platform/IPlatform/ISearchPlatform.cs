using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.SearchModels;

namespace GameTrip.Platform.IPlatform;
public interface ISearchPlatform
{
    Task<IEnumerable<Game>> SearchGameAsync(SearchGameDto dto);
    Task<IEnumerable<Location>> SearchLocationAsync(SearchLocationDto dto);
}
