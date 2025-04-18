﻿namespace GameTrip.Domain.Models.LocationModels;
public class CreateLocationDto
{
    public CreateLocationDto(string? name, string? description, decimal latitude, decimal longitude, Guid authorId)
    {
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        AuthorId = authorId;
    }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
    public Guid AuthorId { get; set; }
}
