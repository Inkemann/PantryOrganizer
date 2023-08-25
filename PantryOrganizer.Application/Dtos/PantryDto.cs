using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Dtos;

public class PantryDto : IIdDto<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public int ItemsCount { get; set; }
}

public class PantryFilterDto
{
    public string? Name { get; set; }
}

public class PantrySortingDto
{
    public SortingParameter Name { get; set; } = new();
    public SortingParameter StorageItemsCount { get; set; } = new();
}
