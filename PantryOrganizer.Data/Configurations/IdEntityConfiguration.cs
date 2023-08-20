using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class IdEntityConfiguration<TData, TId> : IEntityTypeConfiguration<TData>
    where TData : class, IIdEntity<TId>
    where TId : struct, IEquatable<TId>
{
    public virtual void Configure(EntityTypeBuilder<TData> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedOnAdd();
    }
}
