using System.ComponentModel.DataAnnotations;

namespace GameTrip.API.Models;

public class LocationDTO
{
    public LocationDTO(string name, string description, double latitude, double longitude)
    {
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
    }

    [Required]
    public string Name { get; private set; }

    [Required]
    public string Description { get; private set; }

    [Required]
    public double Latitude { get; private set; }

    [Required]
    public double Longitude { get; private set; }
}