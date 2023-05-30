namespace GameTrip.Domain.Models.LocationModels;

public class LocationDto
{
    public LocationDto()
    {

    }
    public LocationDto(string? name, string? description, decimal latitude, decimal longitude, Guid authorId, bool isValidate)
    {
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        AuthorId = authorId;
        IsValidate = isValidate;
    }

    public Guid? Id { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsValidate { get; set; }
}