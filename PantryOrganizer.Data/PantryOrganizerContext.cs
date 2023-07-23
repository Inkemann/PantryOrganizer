using Microsoft.EntityFrameworkCore;

namespace PantryOrganizer.Data;

public class PantryOrganizerContext : DbContext
{
    public PantryOrganizerContext(DbContextOptions<PantryOrganizerContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
