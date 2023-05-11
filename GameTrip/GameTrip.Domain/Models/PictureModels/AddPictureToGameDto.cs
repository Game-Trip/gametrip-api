using Microsoft.AspNetCore.Http;

namespace GameTrip.Domain.Models.PictureModels;
public class AddPictureToGameDto
{
    public Guid? PictureId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? GameId { get; set; }
    public IFormFile? pictureData { get; set; }
}
