using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Extension;

public static class LocationExtension
{

    public static Location ToEntity(this CreateLocationDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Latitude = dto.Latitude,
        Longitude = dto.Longitude
    };

    public static LocationDto ToDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude
    };
    public static List<LocationDto> ToDtoList(this IEnumerable<Location> location) => location.Select(l => l.ToDto()).ToList();
}
