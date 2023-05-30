using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Models.GameModels;
public class GameUpdateRequestNameDto
{
    public Guid IdRequestUpdateGame { get; set; }
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
    public string? NewEditor { get; set; }
    public long? NewReleaseDate { get; set; }
    public LocationNameDto? Location { get; set; }
    public bool? IsAddedLocation { get; set; }
    public PictureDto? Picture { get; set; }
    public bool? IsAddedPicture { get; set; }
}
