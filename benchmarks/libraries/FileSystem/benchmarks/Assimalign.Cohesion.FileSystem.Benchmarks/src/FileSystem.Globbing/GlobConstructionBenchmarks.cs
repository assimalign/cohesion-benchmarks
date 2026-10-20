using System;
using System.IO;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CG = Assimalign.Cohesion.FileSystem.Globbing;
using MG = Microsoft.Extensions.FileSystemGlobbing;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Globbing;

/// <summary>
/// Match a single candidate using a reused matcher or build one per call. Both sides take strings and return
/// the same Boolean; Microsoft's matcher additionally builds its in-memory directory representation.
/// BuildAndMatch includes the first match because Microsoft defers some compilation until execution.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GlobConstructionBenchmarks
{
    private const string Candidate = "src/services/orders/Handler.cs";
    private CG.IGlobMatcher _cohesion = null!;
    private MG.Matcher _extensions = null!;

    [Params("**/*.cs", "src/**/Handler.*")]
    public string Pattern { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = Create();
        _extensions = CreateMicrosoft();
        if (!Match() || !Match_MicrosoftExtensions() || !BuildAndMatch() || !BuildAndMatch_MicrosoftExtensions())
        {
            throw new InvalidOperationException("Glob matchers disagree.");
        }
    }

    private CG.IGlobMatcher Create() => new CG.GlobMatcherBuilder().AddInclude(Glob.Parse(Pattern)).Build();

    private MG.Matcher CreateMicrosoft() => new MG.Matcher(StringComparison.OrdinalIgnoreCase).AddInclude(Pattern);

    [Benchmark, BenchmarkCategory("Reused")]
    public bool Match() => _cohesion.IsMatch(Candidate);

    [Benchmark(Baseline = true), BenchmarkCategory("Reused")]
    public bool Match_MicrosoftExtensions() => MG.MatcherExtensions.Match(_extensions, Candidate).HasMatches;

    [Benchmark, BenchmarkCategory("Build")]
    public bool BuildAndMatch() => Create().IsMatch(Candidate);

    [Benchmark(Baseline = true), BenchmarkCategory("Build")]
    public bool BuildAndMatch_MicrosoftExtensions() => MG.MatcherExtensions.Match(CreateMicrosoft(), Candidate).HasMatches;
}
