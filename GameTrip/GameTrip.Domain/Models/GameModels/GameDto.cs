using GameTrip.Domain.Models.LikeModels.Game;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Models.GameModels;

public class GameDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsValidate { get; set; }

    public IEnumerable<LocationNameDto>? Locations { get; set; }
    public IEnumerable<ListPictureDto>? Pictures { get; set; }
    public IEnumerable<ListLikedGameDto>? LikedGames { get; set; }
}