namespace PantryOrganizer.Data.Models;

public class Pantry : IIdEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<StorageItem> Items { get; set; } = new List<StorageItem>();
}
