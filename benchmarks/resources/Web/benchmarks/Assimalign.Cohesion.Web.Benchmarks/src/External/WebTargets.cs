using Assimalign.Cohesion.Benchmarks.External;

namespace Assimalign.Cohesion.Web.Benchmarks.External;

/// <summary>The Web area's external targets: <c>benchmarks/resources/Web/targets/catalog.json</c>.</summary>
internal static class WebTargets
{
    public static ExternalTargetCatalog Catalog => ExternalTargetCatalog.Load(typeof(WebTargets).Assembly, "benchmarks/resources/Web/targets/catalog.json");
}
