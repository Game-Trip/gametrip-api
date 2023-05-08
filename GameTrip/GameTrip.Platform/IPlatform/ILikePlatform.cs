﻿using GameTrip.Domain.Entities;

namespace GameTrip.Platform.IPlatform;
public interface ILikePlatform
{
    Task AddLikeToLocationAsync(Location location, GameTripUser user, decimal value);
    IEnumerable<LikedLocation> GetAllLikedLocationByLocation(Location location);
    Task<IEnumerable<LikedLocation>> GetAllLikedLocationIncludeAll();
    LikedLocation GetLikeLocation(LikedLocation likedLocation);
    Task RemoveLikeToLocationAsync(Location location, GameTripUser user);
}
