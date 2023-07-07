using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;
using System.Runtime.InteropServices;

namespace GameTrip.Domain.Extension;

public static class LocationExtension
{
    public static Location ToEntity(this CreateLocationDto dto, [Optional] bool force) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Latitude = dto.Latitude,
        Longitude = dto.Longitude,
        AuthorId = dto.AuthorId,
        IsValid = force,
    };

    public static GetLocationDto ToGetLocationDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude,
        AuthorId = location.AuthorId,
        IsValidate = location.IsValid,
        Games = location.Games?.ToList_ListGameDto(),
        Pictures = location.Pictures?.ToEnumerable_ListPictureDto(),
        Comments = location.Comments?.ToList_ListCommentDto(),
        LikedLocations = location.LikedLocations?.ToEnumerable_ListLikedLocationDto(),
    };

    public static LocationNameDto ToLocationNameDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        AuthorId = location.AuthorId,
        IsValidate = location.IsValid
    };
    public static IEnumerable<LocationNameDto> ToEnumerable_LocationNameDto(this ICollection<Location> locations) => locations.Select(l => l.ToLocationNameDto());
    public static IEnumerable<LocationNameDto> ToEnumerable_LocationNameDto(this IEnumerable<Location> locations) => locations.Select(l => l.ToLocationNameDto());

    public static LocationDto ToLocationDto(this Location location) => new()
    {
        Id = location.IdLocation,
        Name = location.Name,
        Description = location.Description,
        Latitude = location.Latitude,
        Longitude = location.Longitude,
        AuthorId = location.AuthorId,
        IsValidate = location.IsValid
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
                Longitude = l.Longitude,
                AuthorId = l.AuthorId,
                IsValidate = l.IsValid
            };
        }).ToList();
    }
    public static IEnumerable<LocationDto> ToEnumerable_LocationDto(this IEnumerable<Location> location)
    {
        return location.Select(l =>
        {
            return new LocationDto()
            {
                Id = l.IdLocation,
                Name = l.Name,
                Description = l.Description,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                AuthorId = l.AuthorId,
                IsValidate = l.IsValid
            };
        }).AsEnumerable();
    }
}