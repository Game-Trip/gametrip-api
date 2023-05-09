using GameTrip.Domain.Models.LocationModels;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.Domain.Models.PictureModels;
public class PictureDto
{
    public Guid PictureId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public FileContentResult? Picture { get; set; }
    public Guid? LocationId { get; set; }
    public LocationNameDto? Location { get; set; }
}
