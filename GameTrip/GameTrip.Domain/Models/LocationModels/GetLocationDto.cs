using GameTrip.Domain.Models.Comment;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.LikeModels.Location;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Models.LocationModels;

public class GetLocationDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public IEnumerable<ListPictureDto>? Pictures { get; set; }
    public IEnumerable<ListGameDto>? Games { get; set; }
    public IEnumerable<ListCommentDto>? Comments { get; set; }
    public IEnumerable<ListLikedLocationDto>? LikedLocations { get; set; }
}