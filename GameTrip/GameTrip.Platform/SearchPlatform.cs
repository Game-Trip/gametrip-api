using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models.SearchModels;
using GameTrip.Platform.IPlatform;

namespace GameTrip.Platform;
public class SearchPlatform : ISearchPlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchPlatform(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Game>> SearchGameAsync(SearchGameDto dto) => await _unitOfWork.Games.SearchGameAsync(dto);
    public async Task<IEnumerable<Location>> SearchLocationAsync(SearchLocationDto dto) => await _unitOfWork.Locations.SearchLocationAsync(dto);
}
