using System;
using System.Collections.Generic;
using System.IO;

using BenchmarkDotNet.Attributes;

using CohesionGlobbing = Assimalign.Cohesion.FileSystem.Globbing;
using ExtensionsGlobbing = Microsoft.Extensions.FileSystemGlobbing;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Globbing;

/// <summary>
/// Filtering a list of 200 relative paths through two include patterns and two exclude patterns
/// (<c>**/*.cs</c>, <c>**/*.json</c>, minus <c>**/bin/**</c> and <c>**/obj/**</c>), against
/// Microsoft.Extensions.FileSystemGlobbing (the baseline). This is the work a file watcher, a build-style file
/// collector or a static-content include list does per candidate.
/// </summary>
/// <remarks>
/// <para>
/// Microsoft.Extensions.FileSystemGlobbing filters a list with <c>Matcher.Match(root, files)</c>, which normalizes
/// every path and walks them as an in-memory directory tree. <see cref="CohesionGlobbing.IGlobMatcher"/> tests one
/// <see cref="FileSystemPath"/> at a time; <see cref="Filter"/> loops over paths already held as
/// <see cref="FileSystemPath"/> (what an <c>IFileSystemInfo.Path</c> gives a caller), and
/// <see cref="Filter_FromStrings"/> starts from the same strings the baseline gets, so it also pays the
/// <see cref="FileSystemPath"/> parse.
/// </para>
/// <para>
/// <c>IGlobMatcher.Match(IFileSystemDirectory)</c> is not measured: it only descends into directories whose own
/// path matches an include pattern, so <c>**/*.cs</c> finds no file below the top level.
/// </para>
/// </remarks>
public class GlobMatcherBenchmarks
{
    private const int PathCount = 200;

    private static readonly string[] Includes = ["**/*.cs", "**/*.json"];
    private static readonly string[] Excludes = ["**/bin/**", "**/obj/**"];

    private CohesionGlobbing.IGlobMatcher _cohesion = null!;
    private ExtensionsGlobbing.Matcher _extensions = null!;
    private string _root = "";
    private string[] _paths = [];
    private FileSystemPath[] _parsedPaths = [];

    [GlobalSetup]
    public void Setup()
    {
        var builder = new CohesionGlobbing.GlobMatcherBuilder();
        _extensions = new ExtensionsGlobbing.Matcher(StringComparison.OrdinalIgnoreCase);

        foreach (string include in Includes)
        {
            builder.AddInclude(Glob.Parse(include));
            _extensions.AddInclude(include);
        }

        foreach (string exclude in Excludes)
        {
            builder.AddExclude(Glob.Parse(exclude));
            _extensions.AddExclude(exclude);
        }

        _cohesion = builder.Build();
        _root = System.IO.Path.GetFullPath("/repo");
        _paths = CreatePaths();
        _parsedPaths = Array.ConvertAll(_paths, path => FileSystemPath.Parse(path));

        int expected = Filter_MicrosoftExtensions();
        if (expected == 0 || Filter() != expected || Filter_FromStrings() != expected)
        {
            throw new InvalidOperationException(
                $"Matchers disagree: Microsoft.Extensions {expected}, Cohesion {Filter()}, Cohesion from strings {Filter_FromStrings()}.");
        }
    }

    [Benchmark(Baseline = true)]
    public int Filter_MicrosoftExtensions()
    {
        int matches = 0;
        foreach (ExtensionsGlobbing.FilePatternMatch _ in ExtensionsGlobbing.MatcherExtensions.Match(_extensions, _root, _paths).Files)
        {
            matches++;
        }

        return matches;
    }

    [Benchmark]
    public int Filter()
    {
        int matches = 0;
        foreach (FileSystemPath path in _parsedPaths)
        {
            if (_cohesion.IsMatch(path))
            {
                matches++;
            }
        }

        return matches;
    }

    [Benchmark]
    public int Filter_FromStrings()
    {
        int matches = 0;
        foreach (string path in _paths)
        {
            if (_cohesion.IsMatch(path))
            {
                matches++;
            }
        }

        return matches;
    }

    /// <summary>A repository-shaped tree: sources, build output, tests, docs and configuration files.</summary>
    private static string[] CreatePaths()
    {
        string[] projects = ["Orders", "Billing", "Shipping", "Catalog", "Identity"];
        string[] files =
        [
            "src/{0}/Program.cs",
            "src/{0}/Models/Order.cs",
            "src/{0}/Services/Handler.cs",
            "src/{0}/appsettings.json",
            "src/{0}/README.md",
            "src/{0}/bin/Release/{0}.dll",
            "src/{0}/bin/Release/{0}.deps.json",
            "src/{0}/obj/Release/{0}.AssemblyInfo.cs",
            "src/{0}/obj/project.assets.json",
            "tests/{0}.Tests/HandlerTests.cs",
            "tests/{0}.Tests/fixtures/order.json",
            "tests/{0}.Tests/obj/Debug/{0}.Tests.AssemblyInfo.cs",
            "docs/{0}/design.md",
            "docs/{0}/diagram.svg",
        ];

        var paths = new List<string>(PathCount);
        for (int i = 0; paths.Count < PathCount; i++)
        {
            string project = projects[i % projects.Length] + (i / projects.Length);
            foreach (string file in files)
            {
                if (paths.Count == PathCount)
                {
                    break;
                }

                paths.Add(string.Format(file, project));
            }
        }

        return [.. paths];
    }
}
