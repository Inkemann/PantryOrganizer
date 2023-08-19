using Microsoft.Extensions.DependencyInjection;
using PantryOrganizer.Application.Query;
using System.Reflection;

namespace PantryOrganizer.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFiltersFromAssemblies(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
    {
        foreach (var assembly in assemblies)
            services.AddFiltersFromAssembly(assembly, lifetime, includeInternalTypes);

        return services;
    }

    public static IServiceCollection AddFiltersFromAssemblyContaining(
        this IServiceCollection services,
        Type type,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddFiltersFromAssembly(type.Assembly, lifetime, includeInternalTypes);

    public static IServiceCollection AddFiltersFromAssemblyContaining<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddFiltersFromAssembly(typeof(T).Assembly, lifetime, includeInternalTypes);

    public static IServiceCollection AddFiltersFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddTypesFromAssembly(
            assembly,
            typeof(IFilter<,>),
            lifetime,
            includeInternalTypes);

    public static IServiceCollection AddSortersFromAssemblies(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
    {
        foreach (var assembly in assemblies)
            services.AddSortersFromAssembly(assembly, lifetime, includeInternalTypes);

        return services;
    }

    public static IServiceCollection AddSortersFromAssemblyContaining(
        this IServiceCollection services,
        Type type,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddSortersFromAssembly(type.Assembly, lifetime, includeInternalTypes);

    public static IServiceCollection AddSortersFromAssemblyContaining<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddSortersFromAssembly(typeof(T).Assembly, lifetime, includeInternalTypes);

    public static IServiceCollection AddSortersFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
        => services.AddTypesFromAssembly(
            assembly,
            typeof(ISorter<,>),
            lifetime,
            includeInternalTypes);

    private static IServiceCollection AddTypesFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        Type typeToScan,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        bool includeInternalTypes = false)
    {
        AssemblyScanner
            .FindTypesInAssembly(assembly, typeToScan, includeInternalTypes)
            .ForEach(scanResult => services.AddScanResult(scanResult, lifetime));

        return services;
    }

    private static IServiceCollection AddScanResult(
        this IServiceCollection services,
        AssemblyScanner.AssemblyScanResult scanResult,
        ServiceLifetime lifetime)
    {
        services.Add(
            new ServiceDescriptor(
                serviceType: scanResult.ScannedType,
                implementationType: scanResult.ResultType,
                lifetime: lifetime));
        services.Add(
            new ServiceDescriptor(
                serviceType: scanResult.ResultType,
                implementationType: scanResult.ResultType,
                lifetime: lifetime));

        return services;
    }
}
