namespace PantryOrganizer.Data.Models;

public class Pantry : IIdEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
