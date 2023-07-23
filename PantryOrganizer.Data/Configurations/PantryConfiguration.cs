using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class PantryConfiguration : IEntityTypeConfiguration<Pantry>
{
    public void Configure(EntityTypeBuilder<Pantry> builder)
    {
        builder.ToTable(nameof(Pantry));

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedOnAdd();

        builder.Property(pantry => pantry.Name)
            .HasMaxLength(StringLength.Medium);
    }
}
