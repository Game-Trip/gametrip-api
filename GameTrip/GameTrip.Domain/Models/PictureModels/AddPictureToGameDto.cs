﻿namespace GameTrip.Domain.Models.PictureModels;
public class AddPictureToGameDto
{
    public Guid? PictureId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? GameId { get; set; }
    public byte[]? pictureData { get; set; }
    public Guid AuthorId { get; set; }
}
