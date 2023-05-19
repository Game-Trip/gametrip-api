namespace GameTrip.Domain.Models.GameModels;
public class GameNameDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsValidate { get; set; }
}
