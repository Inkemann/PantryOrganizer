using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PantryOrganizer.Application.Extensions;
using PantryOrganizer.Data;

namespace PantryOrganizer.Application;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<PantryOrganizerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddAutoMapper(typeof(ServiceConfiguration))
            .AddValidatorsFromAssemblyContaining(typeof(ServiceConfiguration))
            .AddSortersFromAssemblyContaining(typeof(ServiceConfiguration))
            .AddFiltersFromAssemblyContaining(typeof(ServiceConfiguration));
}
