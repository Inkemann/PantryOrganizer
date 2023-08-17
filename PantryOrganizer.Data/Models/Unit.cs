namespace PantryOrganizer.Data.Models;

public class Unit : IIdEntity<Guid>
{
    public Guid Id { get; set; }
    public bool IsBase { get; set; }
    public double? BaseConversionFactor { get; set; }
    public required string Abbreviation { get; set; }
    public required string Name { get; set; }
    public required string AbbreviationPlural { get; set; }
    public required string NamePlural { get; set; }
    public bool IsStorageUnit { get; set; }
    public bool IsRecipeUnit { get; set; }

    public UnitDimensionEnum? DimensionId { get; set; }
    public virtual UnitDimension? Dimension { get; set; }
}

public class UnitDimension
{
    public UnitDimensionEnum Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}

public enum UnitDimensionEnum
{
    Count = 1,
    Weight = 2,
    Volume = 3,
}
