using System;
using System.Collections.Generic;

using Assimalign.Cohesion.Benchmarks.External;

namespace Assimalign.Cohesion.Web.Benchmarks.External;

/// <summary>The Web area's external targets: <c>benchmarks/resources/Web/targets/catalog.json</c>.</summary>
internal static class WebTargets
{
    public static ExternalTargetCatalog Catalog => ExternalTargetCatalog.Load(typeof(WebTargets).Assembly, "benchmarks/resources/Web/targets/catalog.json");

    // The default suite is in-process. Existing external series remain available by explicit opt-in.
    public static IEnumerable<string> AvailableIds(ExternalTargetKind kind)
        => string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("COHESION_BENCHMARK_TARGETS"))
            ? Array.Empty<string>()
            : Catalog.AvailableIds(kind);
}
