namespace PantryOrganizer.Data.Models;

public class StorageItem : IIdEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Note { get; set; }
    public decimal Quantity { get; set; }
    public double? RemainingPercentage { get; set; }
    public DateTimeOffset StoredDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Ean { get; set; }

    public Guid UnitId { get; set; }
    public virtual Unit? Unit { get; set; }
    public Guid PantryId { get; set; }
    public virtual Pantry? Pantry { get; set; }
}
