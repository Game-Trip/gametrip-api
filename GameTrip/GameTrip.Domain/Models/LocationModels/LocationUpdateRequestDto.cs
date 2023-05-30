using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Models.LocationModels;
public class LocationUpdateRequestDto
{
    public Guid LocationId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public Guid? IdGame { get; set; }
    public Game? Game { get; set; }
    public bool? isAddedGame { get; set; }
}
