﻿namespace GameTrip.Domain.Entities;

public class LikedLocation
{
    public Guid IdLikedLocation { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    public Guid UserId { get; set; }
    public GameTripUser? User { get; set; }

    public decimal Vote { get; set; }
}