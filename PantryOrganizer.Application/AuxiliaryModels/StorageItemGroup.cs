using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.AuxiliaryModels;

public class StorageItemGroup
{
    public int ItemCount { get; set; }

    public required string Name { get; set; }
    public decimal Quantity { get; set; }

    public Guid PantryId { get; set; }

    public required Unit Unit { get; set; }
}
