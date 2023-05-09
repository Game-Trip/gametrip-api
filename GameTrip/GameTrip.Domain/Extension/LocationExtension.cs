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
        Pictures = location.Pictures.ToListListDto(),
        Comments = location.Comments,
        LikedLocations = location.LikedLocations.ToListLikedLocationDto(),
    };

    public static LocationNameDto ToLocationNameDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name
    };
    public static IEnumerable<LocationNameDto> ListToLocationNameDto(this ICollection<Location> locations) => locations.Select(l => l.ToLocationNameDto());

    public static LocationDto ToLocationDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude
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