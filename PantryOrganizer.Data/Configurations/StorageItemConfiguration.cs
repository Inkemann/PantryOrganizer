using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class StorageItemConfiguration : GuidEntityConfiguration<StorageItem>
{
    public override void Configure(EntityTypeBuilder<StorageItem> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(StorageItem));

        builder.Property(storageItem => storageItem.Name)
            .HasMaxLength(StringLength.Medium);

        builder.Property(storageItem => storageItem.Note)
            .HasMaxLength(StringLength.Long);

        builder.Property(storageItem => storageItem.Quantity)
            .HasPrecision(12, 4);

        builder.Property(storageItem => storageItem.RemainingPercentage)
            .HasPrecision(5, 4);

        builder.Property(storageItem => storageItem.Ean)
            .HasMaxLength(StringLength.Short);
    }
}
