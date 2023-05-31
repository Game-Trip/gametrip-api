namespace GameTrip.Domain.Models.LocationModels;
public class LocationNameDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsValidate { get; set; }
}
