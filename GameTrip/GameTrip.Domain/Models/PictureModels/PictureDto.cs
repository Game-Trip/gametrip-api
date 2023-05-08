using Microsoft.AspNetCore.Mvc;

namespace GameTrip.Domain.Models.PictureModels;
public class PictureDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public FileContentResult? Picture { get; set; }
}
