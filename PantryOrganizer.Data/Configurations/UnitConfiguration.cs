using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class UnitConfiguration : GuidEntityConfiguration<Unit>
{
    public override void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable(nameof(Unit));

        builder.HasKey(model => model.Id);

        builder.Property(model => model.Name)
            .HasMaxLength(StringLength.Short);

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
