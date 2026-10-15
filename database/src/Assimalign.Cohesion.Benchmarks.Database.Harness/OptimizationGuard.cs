using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Checks actual loaded assemblies, not build configuration labels.</summary>
public static class OptimizationGuard
{
    public static IReadOnlyList<OptimizationAssembly> Inspect()
        => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => (a.GetName().Name ?? "").StartsWith("Assimalign.Cohesion.", StringComparison.Ordinal)
                && !(a.GetName().Name ?? "").StartsWith("Assimalign.Cohesion.Benchmarks.", StringComparison.Ordinal))
            .OrderBy(a => a.GetName().Name, StringComparer.Ordinal)
            .Select(a => new OptimizationAssembly(a.GetName().Name!,
                a.GetCustomAttribute<DebuggableAttribute>()?.IsJITOptimizerDisabled != true,
                a.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown",
                a.IsDynamic ? null : a.Location)).ToArray();
    public static void RequireOptimized(IReadOnlyList<OptimizationAssembly> assemblies)
    {
        if (assemblies.Count == 0) throw new InvalidOperationException("No Cohesion assemblies loaded; optimization could not be verified.");
        var bad = assemblies.Where(a => !a.Optimized).Select(a => a.Name).ToArray();
        if (bad.Length > 0) throw new InvalidOperationException("Official run refused: Debug Cohesion assemblies: " + string.Join(", ", bad));
    }
}
