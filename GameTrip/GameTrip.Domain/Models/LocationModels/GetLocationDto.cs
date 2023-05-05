using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.Domain.Models.LocationModels;

public class GetLocationDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<ListGameDto>? Games { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<LikedLocation>? LikedLocations { get; set; }
}