using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>
/// Options for one dataset generation.
/// </summary>
public sealed class DatasetGenerationOptions
{
    /// <summary>The default seed of every benchmark dataset.</summary>
    public const ulong DefaultSeed = 20261015;

    /// <summary>The model to generate.</summary>
    public required DatasetModel Model { get; init; }

    /// <summary>
    /// The scale factor. Scale 1 is about 1 GB of raw output; the benchmark scale is 10.
    /// </summary>
    public required double Scale { get; init; }

    /// <summary>
    /// The root directory. A generator writes <c>&lt;root&gt;/&lt;model&gt;/sf&lt;scale&gt;/...</c> beneath it,
    /// plus <c>manifest.json</c> in that dataset directory.
    /// </summary>
    public required string OutputDirectory { get; init; }

    /// <summary>The seed every value is derived from.</summary>
    public ulong Seed { get; init; } = DefaultSeed;

    /// <summary>The number of partitions generated in parallel.</summary>
    public int Threads { get; init; } = Environment.ProcessorCount;

    /// <summary>Whether an existing dataset directory may be replaced.</summary>
    public bool Overwrite { get; init; }
}
