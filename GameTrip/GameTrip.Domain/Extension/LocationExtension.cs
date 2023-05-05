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

    public static GetLocationDto ToDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude,
        Games = location.Games.ToDtoList(),
        Pictures = location.Pictures,
        Comments = location.Comments,
        LikedLocations = location.LikedLocations
    };

    public static List<LocationDto> ToDtoList(this IEnumerable<Location> location)
    {
        return location.Select(l =>
        {
            return new LocationDto()
            {
                Id = l.IdLocation,
                Name = l.Name,
                Description = l.Description,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            };
        }).ToList();
    }
}