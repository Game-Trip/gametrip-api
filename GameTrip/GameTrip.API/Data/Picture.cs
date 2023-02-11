namespace GameTrip.API.Data;

public class Picture
{
    public Guid IdPicture { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }

    public ICollection<Location>? Locations { get; set; }
    public ICollection<Game>? Games { get; set; }
}
