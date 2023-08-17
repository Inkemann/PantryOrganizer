using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PantryOrganizer.Data;

namespace PantryOrganizer.Application;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<PantryOrganizerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}
