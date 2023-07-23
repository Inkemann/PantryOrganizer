using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Data.Configurations;

internal class GuidEntityConfiguration<TData> : IEntityTypeConfiguration<TData>
    where TData : class, IIdEntity<Guid>
{
    public virtual void Configure(EntityTypeBuilder<TData> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedOnAdd();
    }
}
