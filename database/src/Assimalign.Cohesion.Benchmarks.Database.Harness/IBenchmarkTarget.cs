using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One engine access path. All methods must observe cancellation and finish cleanup before returning.</summary>
public interface IBenchmarkTarget : IAsyncDisposable
{
    TargetDescriptor Descriptor { get; }
    /// <summary>Every model operation must have an explicit declaration; omissions are configuration errors.</summary>
    IReadOnlyDictionary<string, OperationSupport> Support { get; }
    ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken);
    ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory,
        IProgress<LoadProgress>? progress, CancellationToken cancellationToken);
    ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken);
    ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken);
    ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken);
    ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken);
    /// <summary>A real handshake, plus a trivial statement only where the dialect supports it.</summary>
    ValueTask PingAsync(CancellationToken cancellationToken);
}
