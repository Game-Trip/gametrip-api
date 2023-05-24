﻿using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.Domain.Extension;
public static class LocationUpdateRequestExtension
{
    public static RequestLocationUpdate ToEntity(this LocationUpdateRequestDto dto)
    {
        return new()
        {
            LocationId = dto.LocationId,
            Name = dto.Name ?? null,
            Description = dto.Description ?? null,
            Latitude = dto.Latitude ?? null,
            Longitude = dto.Longitude ?? null,
        };
    }

    public static LocationUpdateRequestNameDto ToLocationUpdateRequestNameDto(this RequestLocationUpdate requestLocationUpdate)
    {
        return new()
        {
            IdRequestUpdateLocation = requestLocationUpdate.IdRequestLocationUpdate,
            NewName = requestLocationUpdate.Name ?? null,
            NewDescription = requestLocationUpdate.Description ?? null,
            NewLatitude = requestLocationUpdate.Latitude ?? null,
            NewLongitude = requestLocationUpdate.Longitude ?? null,
            NewPictures = requestLocationUpdate.Pictures?.ToEnumerable_PictureDto(),
            NewGames = requestLocationUpdate.Games?.ToEnumerable_GameNameDto()
        };
    }

    public static IEnumerable<LocationUpdateRequestNameDto> ToEnumerable_LocationUpdateRequestNameDto(this ICollection<RequestLocationUpdate> requestLocationUpdates) => requestLocationUpdates.Select(rl => rl.ToLocationUpdateRequestNameDto());

    public static ListLocationUpdateRequest ToListLocationUpdateRequest(this Location location)
    {
        return new()
        {
            LocationId = location.IdLocation,
            Location = location.ToGetLocationDto() ?? null,
            RequestUpdate = location.RequestLocationUpdates.ToEnumerable_LocationUpdateRequestNameDto() ?? null
        };
    }
}
