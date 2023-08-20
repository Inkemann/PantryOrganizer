using Microsoft.EntityFrameworkCore;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data;

public class PantryOrganizerContext : DbContext
{
    public PantryOrganizerContext(DbContextOptions<PantryOrganizerContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<UnitDimension>().HasData(
            Enum.GetValues(typeof(UnitDimensionEnum))
                .Cast<UnitDimensionEnum>()
                .Select(entry => new UnitDimension()
                {
                    Id = entry,
                    Name = entry.ToString(),
                }));

        modelBuilder.Entity<Unit>().HasData(
            new Unit
            {
                Id = Guid.Parse("84D92E02-A45A-425C-B139-4BEE471500B9"),
                IsBase = true,
                BaseConversionFactor = 1d,
                Abbreviation = "pc.",
                Name = "Piece",
                AbbreviationPlural = "pcs.",
                NamePlural = "Pieces",
                DimensionId = UnitDimensionEnum.Count,
            },
            new Unit
            {
                Id = Guid.Parse("2FB42003-5924-48C1-9684-BE445A0DA347"),
                IsBase = false,
                BaseConversionFactor = 1d / 12d,
                Abbreviation = "dz.",
                Name = "Dozen",
                AbbreviationPlural = "dzs.",
                NamePlural = "Dozens",
                DimensionId = UnitDimensionEnum.Count,
            },
            new Unit
            {
                Id = Guid.Parse("0F915C81-AC17-418A-95DD-0DB852CC1AF7"),
                IsBase = true,
                BaseConversionFactor = 1d,
                Abbreviation = "g",
                Name = "Gram",
                AbbreviationPlural = "g",
                NamePlural = "Grams",
                DimensionId = UnitDimensionEnum.Weight,
            },
            new Unit
            {
                Id = Guid.Parse("441A63F4-0211-41FD-A4B5-F4D10DDFD7C7"),
                IsBase = false,
                BaseConversionFactor = 1000d,
                Abbreviation = "kg",
                Name = "Kilogram",
                AbbreviationPlural = "kg",
                NamePlural = "Kilograms",
                DimensionId = UnitDimensionEnum.Weight,
            },
            new Unit
            {
                Id = Guid.Parse("C44A6FE7-4548-4754-944B-88E1DBF5E77A"),
                IsBase = false,
                BaseConversionFactor = 1d / 1000d,
                Abbreviation = "ml",
                Name = "Milliliter",
                AbbreviationPlural = "ml",
                NamePlural = "Milliliters",
                DimensionId = UnitDimensionEnum.Volume,
            },
            new Unit
            {
                Id = Guid.Parse("4A7F38CA-6DA4-4256-9D27-8C761FD39CE1"),
                IsBase = true,
                BaseConversionFactor = 1d,
                Abbreviation = "l",
                Name = "Liter",
                AbbreviationPlural = "l",
                NamePlural = "Liters",
                DimensionId = UnitDimensionEnum.Volume,
            },
            new Unit
            {
                Id = Guid.Parse("584DFC68-3E8E-4923-9F9C-B9B9B2F78381"),
                IsBase = false,
                BaseConversionFactor = 15d / 1000d,
                Abbreviation = "tbsp.",
                Name = "Tablespoon",
                AbbreviationPlural = "tbsps.",
                NamePlural = "Tablespoons",
                DimensionId = UnitDimensionEnum.Volume,
            },
            new Unit
            {
                Id = Guid.Parse("20A0BDA9-CECF-4479-9F6F-D0EF75A9798F"),
                IsBase = false,
                BaseConversionFactor = 5d / 1000d,
                Abbreviation = "tsp.",
                Name = "Teaspoon",
                AbbreviationPlural = "tsps.",
                NamePlural = "Teaspoons",
                DimensionId = UnitDimensionEnum.Volume,
            });
    }
}
