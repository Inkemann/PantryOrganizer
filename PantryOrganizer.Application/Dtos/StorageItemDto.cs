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

    public Guid? UnitId { get; set; }
    public UnitDimensionEnumDto UnitDimensionId { get; set; }
    public UnitDto? Unit { get; set; }

    public bool IsExpired
        => ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now;
    public bool IsCloseToExpiration
        => ExpirationDate.HasValue
            && ExpirationDate.Value.Subtract(DateTime.Now) <= new TimeSpan(7, 0, 0, 0);

    public decimal? RemainingQuantity => (decimal)(RemainingPercentage ?? 1d) * Quantity;
}

public class StorageItemFilterDto
{
    public string? Name { get; set; }
}

public class StorageItemSortingDto
{
    public SortingParameter Name { get; set; } = new();
    public SortingParameter Quantity { get; set; } = new();
}

public class StorageItemGroupDto
{
    public int? ItemCount { get; set; }

    public string? Name { get; set; }
    public decimal? Quantity { get; set; }

    public Guid? PantryId { get; set; }

    public Guid? UnitId { get; set; }
    public UnitDimensionEnumDto UnitDimensionId { get; set; }
    public UnitDto? Unit { get; set; }

    public IEnumerable<StorageItemDto>? Items { get; set; }
    public bool IsExpanded { get; set; }
}

public class StorageItemGroupFilterDto
{
    public string? Name { get; set; }
}

public class StorageItemGroupSortingDto
{
    public SortingParameter Name { get; set; } = new();
    public SortingParameter Quantity { get; set; } = new();
}
