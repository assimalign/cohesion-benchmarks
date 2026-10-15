using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One Hz host/container sampler. Linux observes the VM/host visible through procfs; Windows uses GetSystemTimes.</summary>
public sealed class ResourceSampler : IAsyncDisposable
{
    private readonly CancellationTokenSource stop = new();
    private readonly List<ResourceSample> samples = [];
    private readonly Task worker;
    private (ulong Idle, ulong Total)? previous;
    public ResourceSampler(string? container) { previous = Cpu(); worker = LoopAsync(container); }
    public IReadOnlyList<ResourceSample> Samples => samples.ToArray();
    private async Task LoopAsync(string? container)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        try
        {
            while (await timer.WaitForNextTickAsync(stop.Token).ConfigureAwait(false))
            {
                string? telemetry = Environment.GetEnvironmentVariable("DBBENCH_HOST_TELEMETRY");
                if (telemetry != null)
                {
                    try
                    {
                        var sample = JsonSerializer.Deserialize(File.ReadAllText(telemetry), ResultJsonContext.Default.ResourceSample);
                        if (sample != null && (DateTimeOffset.UtcNow - sample.Timestamp).TotalSeconds < 10)
                        { samples.Add(sample); continue; }
                    }
                    catch (Exception ex) when (ex is IOException or JsonException) { }
                    samples.Add(new(DateTimeOffset.UtcNow, null, null, null, "Host telemetry unavailable or stale."));
                    continue;
                }
                var current = Cpu();
                double? host = previous is { } p && current is { } c && c.Total > p.Total
                    ? 100d * (1d - (double)(c.Idle - p.Idle) / (c.Total - p.Total)) : null;
                previous = current;
                string? raw = container == null ? null : await ProcessProbe.ReadAsync("podman",
                    ["stats", "--no-stream", "--format", "json", container], stop.Token).ConfigureAwait(false);
                double? cpu = null;
                string? memory = null;
                string? diagnostic = container == null ? "No engine container named." : raw == null ? "podman stats unavailable." : null;
                if (raw != null)
                {
                    try
                    {
                        using var json = JsonDocument.Parse(raw);
                        var item = json.RootElement.ValueKind == JsonValueKind.Array ? json.RootElement[0] : json.RootElement;
                        foreach (var property in item.EnumerateObject())
                        {
                            string key = property.Name.ToLowerInvariant();
                            if (key is "cpu" or "cpu_percent" or "cpupercent")
                            {
                                if (double.TryParse(property.Value.ToString().TrimEnd('%'), NumberStyles.Float,
                                    CultureInfo.InvariantCulture, out double n)) cpu = n;
                            }
                            if (key is "mem_usage" or "memusage" or "memory") memory = property.Value.ToString();
                        }
                    }
                    catch (Exception ex) when (ex is JsonException or InvalidOperationException or IndexOutOfRangeException)
                    { diagnostic = "Unrecognized podman stats: " + ex.Message; }
                }
                samples.Add(new(DateTimeOffset.UtcNow, host, cpu, memory, diagnostic));
            }
        }
        catch (OperationCanceledException) when (stop.IsCancellationRequested) { }
    }
    private static (ulong Idle, ulong Total)? Cpu()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                if (GetSystemTimes(out ulong idle, out ulong kernel, out ulong user)) return (idle, kernel + user);
            }
            else if (File.Exists("/proc/stat"))
            {
                using var reader = File.OpenText("/proc/stat");
                string[] fields = reader.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                ulong total = 0;
                for (int i = 1; i <= 8 && i < fields.Length; i++) total += ulong.Parse(fields[i], CultureInfo.InvariantCulture);
                return (ulong.Parse(fields[4], CultureInfo.InvariantCulture) + ulong.Parse(fields[5], CultureInfo.InvariantCulture), total);
            }
        }
        catch (IOException) { }
        return null;
    }
    [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetSystemTimes(out ulong idle, out ulong kernel, out ulong user);
    public async ValueTask DisposeAsync()
    {
        await stop.CancelAsync().ConfigureAwait(false);
        await worker.ConfigureAwait(false);
        stop.Dispose();
    }
}
