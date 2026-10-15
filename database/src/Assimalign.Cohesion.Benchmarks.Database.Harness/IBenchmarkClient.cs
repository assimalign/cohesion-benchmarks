using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One closed-loop client. A call consumes the complete response and measures one logical operation.</summary>
public interface IBenchmarkClient : IAsyncDisposable
{
    ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink,
        CancellationToken cancellationToken);
}
