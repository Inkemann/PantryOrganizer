using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PantryOrganizer.Data;

internal class PantryOrganizerContextFactory : IDesignTimeDbContextFactory<PantryOrganizerContext>
{
    public PantryOrganizerContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<PantryOrganizerContext>();
        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        return new PantryOrganizerContext(builder.Options);
    }
}
