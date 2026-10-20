using System;
using System.Diagnostics.Tracing;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging.EventSource;

/// <summary>
/// EventSource event emission, payload mapping and forwarding into a logging sink, or filtering
/// at the source. Microsoft Logging.EventSource sends logs in the opposite direction and is not
/// a counterpart. An owned source and factory are disposed after every benchmark fixture.
/// </summary>
public class EventForwardingBenchmarks
{
    private OrderEventSource _source = null!;
    private ILoggerFactory _factory = null!;
    private CountingProvider _provider = null!;

    [Params(false, true)] public bool Enabled { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _source = new OrderEventSource();
        _provider = new CountingProvider();
        var options = new EventSourceForwardingOptions();
        options.Sources.Clear();
        options.Sources.Add(_source.Name);
        _factory = new LoggerFactoryBuilder().AddProvider(_provider)
            .SetMinimumLevel(Enabled ? LogLevel.Information : LogLevel.Warning)
            .AddEventSourceForwarding(options).Build();
        int before = _provider.Count;
        Emit();
        if (_provider.Count - before != (Enabled ? 1 : 0))
            throw new InvalidOperationException("Event forwarding/filtering mismatch.");
        if (Enabled && (_provider.LastEntry is not { } entry || entry.Category != _source.Name ||
            !Equals(entry.Attributes["orderId"], 42) || !Equals(entry.Attributes["route"], "orders/42")))
            throw new InvalidOperationException("Event payload mapping mismatch.");
    }

    [GlobalCleanup] public void Cleanup() { _factory.Dispose(); _source.Dispose(); }
    [Benchmark] public int Emit() { _source.Processed(42, "orders/42"); return _provider.Count; }

    public sealed class OrderEventSource() : System.Diagnostics.Tracing.EventSource("CohesionBenchmarks.Logging.Orders")
    {
        [Event(1, Level = EventLevel.Informational)]
        public void Processed(int orderId, string route)
        {
            if (IsEnabled()) WriteEvent(1, orderId, route);
        }
    }

    private sealed class CountingProvider : LoggerProvider
    {
        public int Count;
        public ILoggerEntry? LastEntry;
        public override string Name => "counting";
        protected override Logger CreateCore(string category) => new CountingLogger(category, this);
    }

    private sealed class CountingLogger(string category, CountingProvider provider) : Logger(category)
    {
        protected override void WriteCore(ILoggerEntry entry)
        {
            provider.LastEntry = entry;
            provider.Count++;
        }
        protected override IScopedLogger BeginScopeCore(ILoggerEntry entry) => new CountingScope(Category, entry.Id, provider);
    }

    private sealed class CountingScope(string category, LogId parent, CountingProvider provider) : ScopedLogger(category, parent)
    {
        protected override void WriteCore(ILoggerEntry entry) => provider.Count++;
        protected override IScopedLogger BeginScopeCore(ILoggerEntry entry) => new CountingScope(Category, entry.Id, provider);
    }
}
