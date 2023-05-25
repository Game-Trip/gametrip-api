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
    public IEnumerable<LocationNameDto>? NewLocations { get; set; }
    public IEnumerable<PictureDto>? NewPictures { get; set; }
}
