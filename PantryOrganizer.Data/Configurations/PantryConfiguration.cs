using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class PantryConfiguration : IdEntityConfiguration<Pantry, Guid>
{
    public override void Configure(EntityTypeBuilder<Pantry> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(Pantry));

        builder.Property(pantry => pantry.Name)
            .HasMaxLength(StringLength.Medium);
    }
}
