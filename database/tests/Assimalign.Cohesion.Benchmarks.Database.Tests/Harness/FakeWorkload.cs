using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

internal sealed class FakeClock : TimeProvider
{
    private long ticks;
    public override long TimestampFrequency => TimeSpan.TicksPerSecond;
    public override long GetTimestamp() => Interlocked.Read(ref ticks);
    public override DateTimeOffset GetUtcNow() => DateTimeOffset.UnixEpoch.AddTicks(GetTimestamp());
    public void Advance(TimeSpan amount) => Interlocked.Add(ref ticks, amount.Ticks);
}
internal sealed class FakeWorkload(FakeClock clock) : IBenchmarkModel, IBenchmarkTarget, IBenchmarkClient
{
    internal static DatasetManifest Manifest => new() { GeneratorVersion = 1, Model = DatasetModel.Sql,
        Name = "test", Scale = .01, Seed = 42, Tables = [] };
    public TimeSpan ParameterCost { get; set; } = TimeSpan.Zero;
    public TimeSpan ExecutionCost { get; set; } = TimeSpan.FromMilliseconds(1);
    public int Calls { get; private set; }
    public bool Fail { get; set; }
    public string Id => "sql";
    public DatasetModel DatasetModel => DatasetModel.Sql;
    public IReadOnlyList<OperationDescriptor> Operations { get; } = [new("op", "point read", true, TimeoutClass.Point, 100, 100)];
    public IReadOnlyList<TargetDescriptor> Targets => [Descriptor];
    public TargetDescriptor Descriptor { get; } = new("fake", "Fake", true, "Fake", "test");
    public IReadOnlyDictionary<string, OperationSupport> Support { get; set; } = new Dictionary<string, OperationSupport> { ["op"] = new(SupportLevel.Native) };
    public IBenchmarkTarget CreateTarget(string id, TargetOptions options) => this;
    public IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation, int clientIndex,
        ParameterStreamContext context)
        => new Stream(clock, ParameterCost, seed, operation, clientIndex);
    public ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Calls++;
        clock.Advance(ExecutionCost);
        if (Fail) throw new InvalidOperationException("fake failure");
        sink?.AddRow([(ulong)parameters]);
        return ValueTask.CompletedTask;
    }
    public ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken) => ValueTask.FromResult<IBenchmarkClient>(this);
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    public ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken) => ValueTask.CompletedTask;
    public ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory, IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
        => ValueTask.FromResult<IReadOnlyList<LoadMetric>>([]);
    public ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken) => ValueTask.FromResult<IReadOnlyList<LoadMetric>>([]);
    public ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken) => ValueTask.FromResult(new EngineDescription("1", new Dictionary<string, string>()));
    public ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken) => ValueTask.FromResult(0L);
    public ValueTask PingAsync(CancellationToken cancellationToken) => ValueTask.CompletedTask;
    private sealed class Stream(FakeClock clock, TimeSpan cost, ulong seed, string operation, int client) : IParameterStream
    {
        private readonly DeterministicSequence sequence = new(seed, operation, client);
        public object Next() { clock.Advance(cost); return sequence.Next(); }
    }
}
