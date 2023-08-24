using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Dtos;

public class UnitDto : IIdDto<Guid>
{
    public Guid Id { get; set; }
    public bool IsBase { get; set; }
    public double? BaseConversionFactor { get; set; }
    public string? Abbreviation { get; set; }
    public string? Name { get; set; }
    public string? AbbreviationPlural { get; set; }
    public string? NamePlural { get; set; }
    public UnitDimensionEnumDto? Dimension { get; set; }
}

public enum UnitDimensionEnumDto
{
    Count = 1,
    Weight = 2,
    Volume = 3,
}

public class UnitFilterDto { }

public class UnitSortingDto
{
    public SortingParameter ConversionFactor { get; set; } = new();
    public SortingParameter Dimension { get; set; } = new();
}
