﻿using Microsoft.AspNetCore.Http;

namespace GameTrip.Domain.Models.PictureModels;
public class AddPictureToLocationDto
{
    public Guid? PictureId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public IFormFile? pictureData { get; set; }
}
