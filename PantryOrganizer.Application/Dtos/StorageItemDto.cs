using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Dtos;

public class StorageItemDto : IIdDto<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public int? Amount { get; set; }
    public decimal? Quantity { get; set; }
    public double? RemainingPercentage { get; set; }
    public DateTimeOffset? StoredDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Ean { get; set; }

    public Guid? PantryId { get; set; }
    public string? PantryName { get; set; }

    public Guid? UnitId { get; set; }
    public UnitDto? Unit { get; set; }
}

public class StorageItemFilterDto
{
    public string? Name { get; set; }
}

public class StorageItemSortingDto
{
    public SortingParameter? Name { get; set; }
}
