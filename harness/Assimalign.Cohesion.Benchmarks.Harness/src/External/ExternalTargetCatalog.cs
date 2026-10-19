using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>
/// An area's <c>targets/catalog.json</c>: the external applications its benchmarks compare against.
/// </summary>
/// <remarks>
/// <para>A benchmark selects targets with <c>[ParamsSource]</c> over <see cref="AvailableIds"/>, so a target whose
/// prerequisites are missing on this machine (no node on PATH, no container runtime, server not published) is left
/// out of the run instead of failing it. <see cref="Unavailable"/> reports why, and the harness prints it.</para>
/// <para>Set <c>COHESION_BENCHMARK_TARGETS</c> to a comma-separated list of ids to restrict a run further.</para>
/// </remarks>
public sealed class ExternalTargetCatalog
{
    private static readonly ConcurrentDictionary<string, ExternalTargetCatalog> Cache = new(StringComparer.OrdinalIgnoreCase);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    private readonly Dictionary<string, string> _unavailable = new(StringComparer.Ordinal);

    private ExternalTargetCatalog(string path, string repositoryRoot, string cohesionVersion, IReadOnlyList<ExternalTarget> targets)
    {
        CatalogPath = path;
        CatalogDirectory = Path.GetDirectoryName(path)!;
        RepositoryRoot = repositoryRoot;
        CohesionVersion = cohesionVersion;
        Targets = targets;

        string[]? selected = System.Environment.GetEnvironmentVariable("COHESION_BENCHMARK_TARGETS")?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (ExternalTarget target in targets)
        {
            string? reason = selected is { Length: > 0 } && !selected.Contains(target.Id, StringComparer.Ordinal)
                ? "not in COHESION_BENCHMARK_TARGETS"
                : ExternalTargetAvailability.Check(target, this);
            if (reason is not null)
            {
                _unavailable[target.Id] = reason;
                Console.WriteLine($"// external target '{target.Id}' skipped: {reason}");
            }
        }
    }

    /// <summary>The catalog file.</summary>
    public string CatalogPath { get; }

    /// <summary>The catalog's directory (<c>{catalogDir}</c>).</summary>
    public string CatalogDirectory { get; }

    /// <summary>This repository's root (<c>{repoRoot}</c>).</summary>
    public string RepositoryRoot { get; }

    /// <summary>The measured cohesion package version (<c>{cohesionVersion}</c>).</summary>
    public string CohesionVersion { get; }

    /// <summary>Every target in the catalog.</summary>
    public IReadOnlyList<ExternalTarget> Targets { get; }

    /// <summary>Targets that cannot run here, with the reason.</summary>
    public IReadOnlyDictionary<string, string> Unavailable => _unavailable;

    /// <summary>
    /// Loads (once per process) the catalog at <paramref name="relativePath"/>, relative to the repository root.
    /// </summary>
    /// <param name="assembly">The benchmark assembly (it knows the repository root and the cohesion version).</param>
    /// <param name="relativePath">For example <c>benchmarks/resources/Web/targets/catalog.json</c>.</param>
    /// <returns>The catalog.</returns>
    public static ExternalTargetCatalog Load(Assembly assembly, string relativePath)
    {
        string path = BenchmarkEnvironment.GetRepositoryPath(assembly, relativePath);
        return Cache.GetOrAdd(path, static (fullPath, owner) =>
        {
            using FileStream stream = File.OpenRead(fullPath);
            CatalogDocument document = JsonSerializer.Deserialize<CatalogDocument>(stream, JsonOptions)
                ?? throw new InvalidDataException($"{fullPath} is empty.");

            if (document.SchemaVersion != 1)
            {
                throw new InvalidDataException($"{fullPath}: schemaVersion {document.SchemaVersion} is not supported (expected 1).");
            }

            List<string> duplicates = document.Targets.GroupBy(t => t.Id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicates.Count > 0)
            {
                throw new InvalidDataException($"{fullPath}: duplicate target ids {string.Join(", ", duplicates)}.");
            }

            return new ExternalTargetCatalog(
                fullPath,
                BenchmarkEnvironment.GetBenchmarksRoot(owner),
                BenchmarkEnvironment.GetCohesionVersion(owner),
                document.Targets);
        }, assembly);
    }

    /// <summary>The ids of targets that can run on this machine, optionally only of one kind.</summary>
    /// <param name="kind">The kind to keep, or null for all.</param>
    /// <returns>The ids, in catalog order.</returns>
    public IEnumerable<string> AvailableIds(ExternalTargetKind? kind = null)
        => Targets.Where(t => !_unavailable.ContainsKey(t.Id) && (kind is null || t.Kind == kind)).Select(t => t.Id);

    /// <summary>Gets a target by id.</summary>
    /// <param name="id">The id.</param>
    /// <returns>The target.</returns>
    public ExternalTarget Get(string id)
        => Targets.FirstOrDefault(t => t.Id == id) ?? throw new KeyNotFoundException($"No target '{id}' in {CatalogPath}.");

    /// <summary>Starts a long-running target and waits until it is ready.</summary>
    /// <param name="id">A target of kind <see cref="ExternalTargetKind.Process"/> or <see cref="ExternalTargetKind.Container"/>.</param>
    /// <returns>The running instance; dispose it to stop the target.</returns>
    public ExternalTargetInstance Start(string id)
    {
        ExternalTarget target = Get(id);
        if (_unavailable.TryGetValue(id, out string? reason))
        {
            throw new InvalidOperationException($"External target '{id}' cannot run here: {reason}.");
        }

        return ExternalTargetInstance.Start(target, this);
    }

    /// <summary>Runs a command target to completion.</summary>
    /// <param name="id">A target of kind <see cref="ExternalTargetKind.Command"/>.</param>
    /// <param name="timeout">How long the run may take; 60 seconds by default.</param>
    /// <returns>The exit code.</returns>
    public int Run(string id, TimeSpan? timeout = null)
    {
        ExternalTarget target = Get(id);
        if (target.Kind != ExternalTargetKind.Command)
        {
            throw new InvalidOperationException($"External target '{id}' is a {target.Kind}, not a command; use Start.");
        }

        Placeholders placeholders = new(this, ports: []);
        return ExternalCommand.Run(
            placeholders.ResolveCommand(target.Command!),
            target.Arguments.Select(placeholders.Resolve),
            target.WorkingDirectory is null ? null : placeholders.Resolve(target.WorkingDirectory),
            target.Environment.ToDictionary(e => e.Key, e => placeholders.Resolve(e.Value)),
            timeout ?? TimeSpan.FromSeconds(60));
    }

    internal static string CurrentPlatform =>
        OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "osx" : "linux";

    internal static string ExecutableSuffix => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty;

    private sealed class CatalogDocument
    {
        public int SchemaVersion { get; init; }

        public List<ExternalTarget> Targets { get; init; } = [];
    }
}
