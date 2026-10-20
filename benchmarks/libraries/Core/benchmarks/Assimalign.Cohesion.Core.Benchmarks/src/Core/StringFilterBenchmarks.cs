using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// Batch prefix routing and extension filtering using ordinal matching on identical candidate lists.
/// BCL baselines spell out the equivalent String loops. SplitAny is not compared: on net10 the
/// Cohesion span helper delegates directly to MemoryExtensions.SplitAny.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StringFilterBenchmarks
{
    private string[] _inputs = null!;
    private string[] _prefixes = null!;
    private string[] _suffixes = null!;

    [Params(false, true)] public bool Match { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _prefixes = ["api/", "assets/", "health/", "src/"];
        _suffixes = [".json", ".html", ".css", ".cs"];
        _inputs = new string[64];
        for (int i = 0; i < _inputs.Length; i++)
            _inputs[i] = Match ? $"src/project/module{i}.cs" : $"lib/project/module{i}.dll";
        if (StartsWithAny() != StartsWithAny_String() || EndsWithAny() != EndsWithAny_String())
            throw new InvalidOperationException("String filter mismatch.");
    }

    [Benchmark, BenchmarkCategory("Prefix")]
    public int StartsWithAny()
    {
        int count = 0;
        foreach (string input in _inputs)
            if (input.StartsWithAny(_prefixes.AsSpan())) count++;
        return count;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Prefix")]
    public int StartsWithAny_String()
    {
        int count = 0;
        foreach (string input in _inputs)
            foreach (string prefix in _prefixes)
                if (input.StartsWith(prefix, StringComparison.Ordinal)) { count++; break; }
        return count;
    }

    [Benchmark, BenchmarkCategory("Suffix")]
    public int EndsWithAny()
    {
        int count = 0;
        foreach (string input in _inputs)
            if (input.EndsWithAny(_suffixes.AsSpan())) count++;
        return count;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Suffix")]
    public int EndsWithAny_String()
    {
        int count = 0;
        foreach (string input in _inputs)
            foreach (string suffix in _suffixes)
                if (input.EndsWith(suffix, StringComparison.Ordinal)) { count++; break; }
        return count;
    }
}
