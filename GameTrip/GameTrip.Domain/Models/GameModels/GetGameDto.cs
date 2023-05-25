using GameTrip.Domain.Models.LikeModels.Game;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Models.GameModels;
public class GetGameDto
{
    public Guid GameId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Editor { get; set; }
    public long? ReleaseDate { get; set; }
    public bool IsValidate { get; set; }
    public Guid AuthorId { get; set; }

    public ICollection<LocationDto>? Locations { get; set; }
    public ICollection<ListPictureDto>? Pictures { get; set; }
    public ICollection<ListLikedGameDto>? LikedGames { get; set; }
}
