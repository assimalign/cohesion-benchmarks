using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Parsing a configuration key into its <see cref="CohesionConfiguration.Path"/> segments. Every string-keyed
/// configuration read and write goes through this parse.
/// </summary>
/// <remarks>
/// Ported from the legacy <c>KeySegmentParsingStrategy</c> benchmark. Its subject, <c>Key.Parse</c> with
/// <c>$label</c> and <c>[index]</c> suffixes, no longer exists: key segments are now split by <c>Path.Parse</c>.
/// Microsoft.Extensions.Configuration keeps keys as flat strings and has no equivalent parse, so there is no
/// baseline.
/// </remarks>
public class PathBenchmarks
{
    /// <summary>The colon form every provider produces, and the mixed-delimiter form the parser also accepts.</summary>
    [Benchmark]
    [Arguments("Service:Timeouts:Request")]
    [Arguments("/Service.Endpoints\0:Url")]
    public CohesionConfiguration.Path Parse(string value) => CohesionConfiguration.Path.Parse(value);
}
