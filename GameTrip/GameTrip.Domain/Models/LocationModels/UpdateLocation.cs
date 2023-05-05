namespace GameTrip.Domain.Models.LocationModels;
public class UpdateLocationDto
{
    public Guid LocationId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
