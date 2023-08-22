using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class UnitConfiguration : IdEntityConfiguration<Unit, Guid>
{
    public override void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable(nameof(Unit));

        builder.HasKey(model => model.Id);

        builder.Property(model => model.Abbreviation)
            .HasMaxLength(StringLength.Short);

        builder.Property(model => model.Name)
            .HasMaxLength(StringLength.Short);

        builder.Property(model => model.AbbreviationPlural)
            .HasMaxLength(StringLength.Short);

        builder.Property(model => model.NamePlural)
            .HasMaxLength(StringLength.Short);

        builder.HasIndex(model => model.DimensionId)
            .IsUnique()
            .HasFilter($"[{nameof(Unit.IsBase)}] = 1");
    }
}

internal class UnitDimensionConfiguration : IEntityTypeConfiguration<UnitDimension>
{
    public void Configure(EntityTypeBuilder<UnitDimension> builder)
    {
        builder.ToTable(nameof(UnitDimension));

        builder.HasKey(model => model.Id);

        builder.Property(model => model.Name)
            .HasMaxLength(StringLength.Short);
    }
}
