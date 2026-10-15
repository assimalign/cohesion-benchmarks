using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal abstract class DocumentsTarget : IBenchmarkTarget
{
    protected readonly TargetOptions Options;
    protected DocumentsTarget(TargetDescriptor descriptor, TargetOptions options)
    {
        if (options.BatchSize <= 0) throw new ArgumentOutOfRangeException(nameof(options.BatchSize));
        Descriptor = descriptor; Options = options; Support = DocumentWorkload.Support(descriptor.Id);
    }
    public TargetDescriptor Descriptor { get; }
    public IReadOnlyDictionary<string, OperationSupport> Support { get; }
    public abstract ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken);
    public virtual ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory,
        IProgress<LoadProgress>? progress, CancellationToken cancellationToken) => throw new NotImplementedException("Loader lands in item 2.");
    public abstract ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken);
    public abstract ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken);
    public abstract ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken);
    public virtual ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken) => throw new NotImplementedException("Operations land in item 3.");
    public abstract ValueTask PingAsync(CancellationToken cancellationToken);
    public abstract ValueTask DisposeAsync();
}
