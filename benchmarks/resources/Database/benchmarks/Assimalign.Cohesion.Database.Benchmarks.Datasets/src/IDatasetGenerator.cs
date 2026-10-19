using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>
/// Generates one model's dataset deterministically from a seed and a scale factor.
/// </summary>
public interface IDatasetGenerator
{
    /// <summary>The model this generator produces.</summary>
    DatasetModel Model { get; }

    /// <summary>The dataset name: <c>commerce</c>, <c>marketplace</c> or <c>social</c>.</summary>
    string Name { get; }

    /// <summary>
    /// Writes the dataset and its manifest and returns the manifest.
    /// </summary>
    ValueTask<DatasetManifest> GenerateAsync(
        DatasetGenerationOptions options,
        IProgress<DatasetProgress>? progress,
        CancellationToken cancellationToken);
}
