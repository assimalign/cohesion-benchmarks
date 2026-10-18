using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// <see cref="Glob.IsMatch(ReadOnlySpan{char})"/> over a batch of paths, half of which match, against the
/// equivalent compiled <see cref="Regex"/> (the baseline). Glob matching sits under file watching,
/// FileSystem.Globbing include/exclude filters and static-file routing, so it runs once per candidate path.
/// </summary>
/// <remarks>
/// Ported from the legacy Core glob suite (<c>Benchmarks.Glob</c> and <c>BaselineRegexIsMatch*</c>). The legacy
/// regex came from a DotNet.Glob token visitor that no longer exists; each pattern's regex is now written out by
/// hand, and <see cref="Setup"/> fails the run if the two ever disagree on the inputs.
/// </remarks>
public class GlobBenchmarks
{
    private const int InputCount = 64;

    /// <summary>The hand-written regex equivalent of each benchmarked pattern.</summary>
    private static readonly Dictionary<string, string> RegexEquivalents = new(StringComparer.Ordinal)
    {
        // Single-character wildcard, a character set and a range (the legacy suite's middle pattern).
        ["p?th/a[bcd]b[e-g].txt"] = @"^p[^/\\]th[/\\]a[bcd]b[e-g]\.txt$",
        // A directory wildcard plus a brace group: the shape of a typical include filter.
        ["src/**/*.{cs,json}"] = @"^src[/\\].*[/\\][^/\\]*\.(?:cs|json)$",
    };

    private Glob _glob = null!;
    private Regex _regex = null!;
    private string[] _inputs = [];

    [Params("p?th/a[bcd]b[e-g].txt", "src/**/*.{cs,json}")]
    public string Pattern { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _glob = Glob.Parse(Pattern);
        _regex = new Regex(RegexEquivalents[Pattern], RegexOptions.Compiled | RegexOptions.CultureInvariant);
        _inputs = CreateInputs(Pattern);

        int globMatches = IsMatch();
        int regexMatches = IsMatch_Regex();

        if (globMatches != InputCount / 2 || regexMatches != InputCount / 2)
        {
            throw new InvalidOperationException(
                $"'{Pattern}': expected {InputCount / 2} matches, glob found {globMatches} and regex {regexMatches}.");
        }
    }

    [Benchmark(Baseline = true)]
    public int IsMatch_Regex()
    {
        int matches = 0;
        foreach (string input in _inputs)
        {
            if (_regex.IsMatch(input))
            {
                matches++;
            }
        }

        return matches;
    }

    [Benchmark]
    public int IsMatch()
    {
        int matches = 0;
        foreach (string input in _inputs)
        {
            if (_glob.IsMatch(input))
            {
                matches++;
            }
        }

        return matches;
    }

    private static string[] CreateInputs(string pattern)
    {
        var random = new Random(17);
        var inputs = new string[InputCount];

        for (int i = 0; i < InputCount; i++)
        {
            bool match = i % 2 == 0;
            inputs[i] = pattern switch
            {
                "p?th/a[bcd]b[e-g].txt" => CharacterClassInput(random, match),
                "src/**/*.{cs,json}" => DirectoryWildcardInput(random, match),
                _ => throw new InvalidOperationException($"No inputs for '{pattern}'."),
            };
        }

        return inputs;
    }

    private static string CharacterClassInput(Random random, bool match)
    {
        char any = (char)random.Next('a', 'z' + 1);
        char set = "bcd"[random.Next(3)];
        char range = (char)random.Next('e', 'g' + 1);

        if (match)
        {
            return $"p{any}th/a{set}b{range}.txt";
        }

        // Each miss fails at a different token: the set, the range, the literal suffix, or the length.
        return random.Next(4) switch
        {
            0 => $"p{any}th/a{(char)random.Next('w', 'z' + 1)}b{range}.txt",
            1 => $"p{any}th/a{set}b{(char)random.Next('h', 'z' + 1)}.txt",
            2 => $"p{any}th/a{set}b{range}.txs",
            _ => $"p{any}{any}th/a{set}b{range}.txt",
        };
    }

    private static string DirectoryWildcardInput(Random random, bool match)
    {
        string directories = string.Join('/', RandomSegments(random, random.Next(1, 4)));
        string name = RandomSegment(random);

        if (match)
        {
            return $"src/{directories}/{name}{(random.Next(2) == 0 ? ".cs" : ".json")}";
        }

        return random.Next(3) switch
        {
            0 => $"lib/{directories}/{name}.cs",
            1 => $"src/{directories}/{name}.txt",
            _ => $"src/{directories}/{name}.csproj",
        };
    }

    private static IEnumerable<string> RandomSegments(Random random, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return RandomSegment(random);
        }
    }

    private static string RandomSegment(Random random)
    {
        Span<char> segment = stackalloc char[random.Next(3, 10)];
        for (int i = 0; i < segment.Length; i++)
        {
            segment[i] = (char)random.Next('a', 'z' + 1);
        }

        return new string(segment);
    }
}
