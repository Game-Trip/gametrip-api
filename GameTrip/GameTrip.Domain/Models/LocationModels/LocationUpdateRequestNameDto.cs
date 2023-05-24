using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Models.LocationModels;
public class LocationUpdateRequestNameDto
{
    public Guid IdRequestUpdateLocation { get; set; }
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
    public decimal? NewLatitude { get; set; }
    public decimal? NewLongitude { get; set; }
    public IEnumerable<PictureDto>? NewPictures { get; set; }
    public IEnumerable<GameNameDto>? NewGames { get; set; }
}
