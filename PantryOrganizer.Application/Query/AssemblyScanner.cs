using System.Collections;
using System.Reflection;

namespace PantryOrganizer.Application.Query;

public class AssemblyScanner : IEnumerable<AssemblyScanner.AssemblyScanResult>
{
    private readonly IEnumerable<Type> types;
    private readonly Type typeToScan;

    public AssemblyScanner(IEnumerable<Type> types, Type typeToScan)
    {
        this.types = types;
        this.typeToScan = typeToScan;
    }

    public static AssemblyScanner FindTypesInAssembly(
        Assembly assembly,
        Type typeToScan,
        bool includeInternalTypes = false)
        => new(includeInternalTypes ? assembly.GetTypes() : assembly.GetExportedTypes(),
            typeToScan);

    public static AssemblyScanner FindValidatorsInAssemblies(
        IEnumerable<Assembly> assemblies,
        Type typeToScan,
        bool includeInternalTypes = false)
    {
        var types = assemblies
            .SelectMany(x => includeInternalTypes ? x.GetTypes() : x.GetExportedTypes())
            .Distinct();
        return new AssemblyScanner(types, typeToScan);
    }

    private IEnumerable<AssemblyScanResult> Execute()
    {
        foreach (var type in types)
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
                continue;

            var matchingInterface = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == (Type?)typeToScan)
                .FirstOrDefault();

            if (matchingInterface != default)
                yield return new AssemblyScanResult(matchingInterface, type);
        }
    }

    public void ForEach(Action<AssemblyScanResult> action)
    {
        foreach (var result in this)
        {
            action(result);
        }
    }

    public IEnumerator<AssemblyScanResult> GetEnumerator()
        => Execute().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public class AssemblyScanResult
    {
        public AssemblyScanResult(Type scannedType, Type resultType)
        {
            ScannedType = scannedType;
            ResultType = resultType;
        }

        public Type ScannedType { get; private set; }

        public Type ResultType { get; private set; }
    }
}
