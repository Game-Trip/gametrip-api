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
    public Guid? PictureId { get; set; }
    public PictureDto? Picture { get; set; }
    public bool? IsAddedPicture { get; set; }
    public Guid? GameId { get; set; }
    public GameNameDto? Game { get; set; }
    public bool? IsAddedGame { get; set; }
}
