namespace GameTrip.API.Data;

public class Location
{
    public Guid IdLocation { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }


    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<Game>? Games { get; set; }
    public ICollection<Comment>? Comments{ get; set; }
    public ICollection<LikedLocation>? LikedLocations { get; set; }
}