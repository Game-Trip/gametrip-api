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

    public static GetLocationDto ToGetLocationDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude,
        Games = location.Games?.ToList_ListGameDto(),
        Pictures = location.Pictures?.ToEnumerable_ListPictureDto(),
        Comments = location.Comments ??= null,
        LikedLocations = location.LikedLocations?.ToEnumerable_ListLikedLocationDto(),
    };

    public static LocationNameDto ToLocationNameDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name
    };
    public static IEnumerable<LocationNameDto> ToEnumerable_LocationNameDto(this ICollection<Location> locations) => locations.Select(l => l.ToLocationNameDto());

    public static LocationDto ToLocationDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude
    };

    public static List<LocationDto> ToList_LocationDto(this IEnumerable<Location> location)
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