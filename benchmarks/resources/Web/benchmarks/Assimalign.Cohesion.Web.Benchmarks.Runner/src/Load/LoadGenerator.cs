using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Runner.Http;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Scenarios;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Load;

internal enum LoadPhase
{
    Warmup = 0,
    Measuring = 1,
    Stopped = 2,
}

/// <summary>The phase every connection reads before it records a response.</summary>
internal sealed class LoadClock
{
    private volatile int _phase;

    public LoadPhase Phase => (LoadPhase)_phase;

    public void Set(LoadPhase phase) => _phase = (int)phase;
}

internal sealed class ConnectionStats
{
    public LatencyHistogram Latency { get; } = new();

    /// <summary>Responses by status class: index 1 = 1xx ... 5 = 5xx.</summary>
    public long[] StatusClasses { get; } = new long[6];

    public long Bytes { get; set; }

    public long Errors { get; set; }

    public long Reconnects { get; set; }

    public void Record(int status, long elapsedTicks)
    {
        Latency.Record(elapsedTicks);
        int statusClass = status / 100;
        if (statusClass is >= 1 and <= 5)
        {
            StatusClasses[statusClass]++;
        }
    }
}

internal sealed record LoadResult(
    long Requests,
    double ElapsedSeconds,
    long Errors,
    long Reconnects,
    long[] StatusClasses,
    long Bytes,
    LatencyHistogram Latency)
{
    public double RequestsPerSecond => ElapsedSeconds <= 0 ? 0 : Requests / ElapsedSeconds;

    public double BytesPerSecond => ElapsedSeconds <= 0 ? 0 : Bytes / ElapsedSeconds;
}

internal sealed record ValidationResult(bool Passed, string? Failure, int Status, int HeadLength, long BodyLength, string Head, string BodyPreview);

/// <summary>
/// Drives a scenario against one server: a validation exchange first, then a fixed number of keep-alive
/// connections that replay their pre-encoded requests through a warmup phase and a measured phase.
/// </summary>
internal static class LoadGenerator
{
    public static async Task<ValidationResult> ValidateAsync(LoadScenario scenario, IPEndPoint endpoint, CancellationToken cancellationToken)
    {
        if (scenario.Kind == LoadKind.WebSocket)
        {
            return await ValidateWebSocketAsync(scenario, endpoint, cancellationToken).ConfigureAwait(false);
        }

        using Socket socket = await ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
        var reader = new HttpResponseReader(socket);
        (SetupResult setup, ResponseCapture? setupCapture) = await RunSetupAsync(scenario, endpoint, socket, reader, cancellationToken).ConfigureAwait(false);
        if (setupCapture is not null && setupCapture.Status is < 200 or >= 400)
        {
            return new ValidationResult(false, $"setup request answered {setupCapture.Status}", setupCapture.Status, setupCapture.HeadLength, setupCapture.BodyLength, setupCapture.Head, Preview(setupCapture));
        }

        HttpRequestTemplate request = scenario.Requests(setup)[0];
        var capture = new ResponseCapture();
        await SendAllAsync(socket, request.Encode(endpoint.ToString()), cancellationToken).ConfigureAwait(false);
        int status = await reader.ReadAsync(request.IsHead, capture, cancellationToken).ConfigureAwait(false);

        string? failure = null;
        if (!scenario.ExpectedStatus.Contains(status))
        {
            failure = $"status {status}, expected {string.Join(" or ", scenario.ExpectedStatus)}";
        }
        else if (scenario.ExpectBodyContains is { } expected && !capture.BodyText.Contains(expected, StringComparison.Ordinal))
        {
            failure = $"body does not contain '{expected}'";
        }
        else if (scenario.ExpectHeaders.FirstOrDefault(header => capture.GetHeader(header) is null) is { } missing)
        {
            failure = $"response has no {missing} header";
        }

        return new ValidationResult(failure is null, failure, status, capture.HeadLength, capture.BodyLength, capture.Head, Preview(capture));
    }

    public static Task<LoadResult> RunAsync(
        LoadScenario scenario,
        IPEndPoint endpoint,
        int connections,
        TimeSpan warmup,
        TimeSpan duration,
        Action onMeasureStart,
        Action onMeasureStop)
    {
        Func<int, LoadClock, ConnectionStats, CancellationToken, Task> connection = scenario.Kind == LoadKind.WebSocket
            ? (index, clock, stats, token) => RunWebSocketConnectionAsync(scenario, endpoint, clock, stats, token)
            : (index, clock, stats, token) => RunHttpConnectionAsync(index, scenario, endpoint, clock, stats, token);

        return RunPhasesAsync(connections, warmup, duration, connection, onMeasureStart, onMeasureStop);
    }

    private static async Task<LoadResult> RunPhasesAsync(
        int connections,
        TimeSpan warmup,
        TimeSpan duration,
        Func<int, LoadClock, ConnectionStats, CancellationToken, Task> connection,
        Action onMeasureStart,
        Action onMeasureStop)
    {
        var clock = new LoadClock();
        using var abort = new CancellationTokenSource();
        var stats = new ConnectionStats[connections];
        var tasks = new Task[connections];
        for (int i = 0; i < connections; i++)
        {
            stats[i] = new ConnectionStats();
            int index = i;
            tasks[i] = Task.Run(() => connection(index, clock, stats[index], abort.Token));
        }

        await Task.Delay(warmup).ConfigureAwait(false);

        onMeasureStart();
        long start = Stopwatch.GetTimestamp();
        clock.Set(LoadPhase.Measuring);

        await Task.Delay(duration).ConfigureAwait(false);

        clock.Set(LoadPhase.Stopped);
        double elapsed = Stopwatch.GetElapsedTime(start).TotalSeconds;
        onMeasureStop();

        // Connections finish the exchange in flight; anything still blocked after a grace period is cut off.
        Task all = Task.WhenAll(tasks);
        if (await Task.WhenAny(all, Task.Delay(TimeSpan.FromSeconds(5))).ConfigureAwait(false) != all)
        {
            abort.Cancel();
            await Task.WhenAny(all, Task.Delay(TimeSpan.FromSeconds(5))).ConfigureAwait(false);
        }

        var latency = new LatencyHistogram();
        long[] statusClasses = new long[6];
        long bytes = 0, errors = 0, reconnects = 0;
        foreach (ConnectionStats connectionStats in stats)
        {
            latency.Add(connectionStats.Latency);
            for (int i = 0; i < statusClasses.Length; i++)
            {
                statusClasses[i] += connectionStats.StatusClasses[i];
            }
            bytes += connectionStats.Bytes;
            errors += connectionStats.Errors;
            reconnects += connectionStats.Reconnects;
        }

        return new LoadResult(latency.Count, elapsed, errors, reconnects, statusClasses, bytes, latency);
    }

    private static async Task RunHttpConnectionAsync(int index, LoadScenario scenario, IPEndPoint endpoint, LoadClock clock, ConnectionStats stats, CancellationToken cancellationToken)
    {
        int depth = scenario.PipelineDepth;
        while (clock.Phase != LoadPhase.Stopped && !cancellationToken.IsCancellationRequested)
        {
            Socket? socket = null;
            try
            {
                socket = await ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
                var reader = new HttpResponseReader(socket);
                (SetupResult setup, _) = await RunSetupAsync(scenario, endpoint, socket, reader, cancellationToken).ConfigureAwait(false);

                IReadOnlyList<HttpRequestTemplate> requests = scenario.Requests(setup);
                bool head = requests[0].IsHead;
                byte[][] batches = requests.Select(request => Repeat(request.Encode(endpoint.ToString()), depth)).ToArray();
                int next = index % batches.Length;
                long lastBytes = reader.BytesReceived;

                while (clock.Phase != LoadPhase.Stopped)
                {
                    byte[] batch = batches[next];
                    next = next + 1 == batches.Length ? 0 : next + 1;

                    long start = Stopwatch.GetTimestamp();
                    await SendAllAsync(socket, batch, cancellationToken).ConfigureAwait(false);
                    for (int i = 0; i < depth; i++)
                    {
                        int status = await reader.ReadAsync(head, null, cancellationToken).ConfigureAwait(false);
                        long elapsed = Stopwatch.GetTimestamp() - start;
                        if (clock.Phase == LoadPhase.Measuring)
                        {
                            stats.Record(status, elapsed);
                            stats.Bytes += reader.BytesReceived - lastBytes;
                        }
                        lastBytes = reader.BytesReceived;
                    }

                    if (reader.CloseRequested)
                    {
                        if (clock.Phase == LoadPhase.Measuring)
                        {
                            stats.Reconnects++;
                        }
                        break;
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception)
            {
                if (clock.Phase == LoadPhase.Measuring)
                {
                    stats.Errors++;
                }
                if (clock.Phase == LoadPhase.Stopped)
                {
                    return;
                }
                await Task.Delay(10, CancellationToken.None).ConfigureAwait(false);
            }
            finally
            {
                socket?.Dispose();
            }
        }
    }

    private static async Task RunWebSocketConnectionAsync(LoadScenario scenario, IPEndPoint endpoint, LoadClock clock, ConnectionStats stats, CancellationToken cancellationToken)
    {
        byte[] message = new byte[scenario.WebSocketMessageSize];
        message.AsSpan().Fill((byte)'w');
        byte[] buffer = new byte[Math.Max(4096, message.Length * 2)];

        while (clock.Phase != LoadPhase.Stopped && !cancellationToken.IsCancellationRequested)
        {
            using var socket = new ClientWebSocket();
            socket.Options.KeepAliveInterval = TimeSpan.Zero;
            try
            {
                await socket.ConnectAsync(new Uri($"ws://{endpoint}{scenario.WebSocketPath}"), cancellationToken).ConfigureAwait(false);
                while (clock.Phase != LoadPhase.Stopped)
                {
                    long start = Stopwatch.GetTimestamp();
                    await socket.SendAsync(message, WebSocketMessageType.Text, endOfMessage: true, cancellationToken).ConfigureAwait(false);
                    int received = 0;
                    ValueWebSocketReceiveResult result;
                    do
                    {
                        result = await socket.ReceiveAsync(buffer.AsMemory(received), cancellationToken).ConfigureAwait(false);
                        received += result.Count;
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    long elapsed = Stopwatch.GetTimestamp() - start;
                    if (clock.Phase == LoadPhase.Measuring)
                    {
                        stats.Record(received == message.Length ? 200 : 500, elapsed);
                        stats.Bytes += received;
                    }
                }

                try
                {
                    using var close = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                    await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, close.Token).ConfigureAwait(false);
                }
                catch
                {
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception)
            {
                if (clock.Phase == LoadPhase.Measuring)
                {
                    stats.Errors++;
                }
                if (clock.Phase == LoadPhase.Stopped)
                {
                    return;
                }
                await Task.Delay(10, CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    private static async Task<ValidationResult> ValidateWebSocketAsync(LoadScenario scenario, IPEndPoint endpoint, CancellationToken cancellationToken)
    {
        using var socket = new ClientWebSocket();
        try
        {
            await socket.ConnectAsync(new Uri($"ws://{endpoint}{scenario.WebSocketPath}"), cancellationToken).ConfigureAwait(false);
            byte[] message = "cohesion-benchmarks websocket ok"u8.ToArray();
            await socket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);
            byte[] buffer = new byte[1024];
            ValueWebSocketReceiveResult result = await socket.ReceiveAsync(buffer.AsMemory(), cancellationToken).ConfigureAwait(false);
            string echoed = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken).ConfigureAwait(false);
            bool passed = echoed == "cohesion-benchmarks websocket ok";
            return new ValidationResult(passed, passed ? null : $"echo returned '{echoed}'", 101, 0, result.Count, "HTTP/1.1 101 Switching Protocols", echoed);
        }
        catch (Exception exception)
        {
            return new ValidationResult(false, exception.GetBaseException().Message, 0, 0, 0, string.Empty, string.Empty);
        }
    }

    private static async Task<(SetupResult Setup, ResponseCapture? Capture)> RunSetupAsync(
        LoadScenario scenario,
        IPEndPoint endpoint,
        Socket socket,
        HttpResponseReader reader,
        CancellationToken cancellationToken)
    {
        if (scenario.Setup is null)
        {
            return (new SetupResult(null, null), null);
        }

        var capture = new ResponseCapture();
        await SendAllAsync(socket, scenario.Setup.Encode(endpoint.ToString()), cancellationToken).ConfigureAwait(false);
        await reader.ReadAsync(scenario.Setup.IsHead, capture, cancellationToken).ConfigureAwait(false);

        string[] cookies = capture.GetHeaders("Set-Cookie")
            .Select(header => header.Split(';', 2)[0].Trim())
            .Where(pair => pair.Length > 0)
            .ToArray();
        string? cookieHeader = cookies.Length == 0 ? null : string.Join("; ", cookies);
        string? token = scenario.SetupBodyIsToken ? capture.BodyText.Trim() : null;
        return (new SetupResult(cookieHeader, token), capture);
    }

    private static async Task<Socket> ConnectAsync(IPEndPoint endpoint, CancellationToken cancellationToken)
    {
        var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        try
        {
            await socket.ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
            return socket;
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    [System.Runtime.CompilerServices.AsyncMethodBuilder(typeof(System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder))]
    private static async ValueTask SendAllAsync(Socket socket, ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        while (!data.IsEmpty)
        {
            int sent = await socket.SendAsync(data, SocketFlags.None, cancellationToken).ConfigureAwait(false);
            data = data[sent..];
        }
    }

    private static byte[] Repeat(byte[] request, int count)
    {
        if (count == 1)
        {
            return request;
        }

        byte[] batch = new byte[request.Length * count];
        for (int i = 0; i < count; i++)
        {
            request.CopyTo(batch, i * request.Length);
        }
        return batch;
    }

    private static string Preview(ResponseCapture capture)
    {
        string text = capture.BodyText;
        return text.Length <= 160 ? text : text[..160] + "...";
    }
}
