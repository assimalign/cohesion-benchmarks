using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Captures runtime facts; immutable build provenance is copied into both images at publish time.</summary>
public static class EnvironmentCapture
{
    public static async Task<EnvironmentSnapshot> CaptureAsync(string? container, string? cohesionRoot, CancellationToken token = default)
    {
        string? inspection = container == null ? null : await ProcessProbe.ReadAsync("podman", ["inspect", container], token).ConfigureAwait(false);
        string? digest = null;
        if (inspection != null)
        {
            using var json = JsonDocument.Parse(inspection);
            var item = json.RootElement[0];
            if (item.TryGetProperty("ImageDigest", out var value)) digest = value.GetString();
            if (string.IsNullOrEmpty(digest) && item.TryGetProperty("Image", out value)) digest = value.GetString();
        }
        var optimization = OptimizationGuard.Inspect();
        string? benchmarkCommit = await ProcessProbe.ReadAsync("git", ["rev-parse", "HEAD"], token).ConfigureAwait(false)
            ?? Environment.GetEnvironmentVariable("DBBENCH_BENCHMARKS_COMMIT");
        string? benchmarkStatus = await ProcessProbe.ReadAsync("git", ["status", "--porcelain"], token).ConfigureAwait(false);
        bool? benchmarkDirty = benchmarkStatus != null ? benchmarkStatus.Length != 0 :
            bool.TryParse(Environment.GetEnvironmentVariable("DBBENCH_BENCHMARKS_DIRTY"), out bool dirty) ? dirty : null;
        string? cohesionCommit = cohesionRoot == null ? null :
            await ProcessProbe.ReadAsync("git", ["-C", cohesionRoot, "rev-parse", "HEAD"], token).ConfigureAwait(false);
        cohesionCommit ??= Environment.GetEnvironmentVariable("DBBENCH_COHESION_COMMIT");
        string cpu = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "unknown";
        long? memory = null;
        if (OperatingSystem.IsWindows())
        {
            var status = new MemoryStatus { Length = (uint)Marshal.SizeOf<MemoryStatus>() };
            if (GlobalMemoryStatusEx(ref status)) memory = checked((long)status.TotalPhysical);
        }
        else
        {
            if (File.Exists("/proc/cpuinfo"))
                cpu = File.ReadLines("/proc/cpuinfo").FirstOrDefault(l => l.StartsWith("model name", StringComparison.Ordinal) ||
                    l.StartsWith("CPU part", StringComparison.Ordinal)) ?? RuntimeInformation.ProcessArchitecture.ToString();
            if (File.Exists("/proc/meminfo"))
            {
                string? line = File.ReadLines("/proc/meminfo").FirstOrDefault(l => l.StartsWith("MemTotal:", StringComparison.Ordinal));
                if (line != null && long.TryParse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1], out long kb)) memory = kb * 1024;
            }
        }
        return new()
        {
            CapturedUtc = DateTimeOffset.UtcNow, OperatingSystem = RuntimeInformation.OSDescription,
            Architecture = RuntimeInformation.ProcessArchitecture.ToString(), Cpu = cpu,
            LogicalProcessors = Environment.ProcessorCount, PhysicalMemoryBytes = memory,
            BenchmarksCommit = benchmarkCommit, CohesionCheckoutCommit = cohesionCommit,
            BenchmarksDirty = benchmarkDirty,
            CohesionBuiltCommits = optimization.Select(a => a.InformationalVersion.Split('+').Last())
                .Where(s => s.Length >= 9 && s.All(Uri.IsHexDigit)).Distinct().ToArray(),
            Optimization = optimization, Container = container, ContainerInspectJson = inspection, ImageDigest = digest,
            CgroupMemoryMax = Read("/sys/fs/cgroup/memory.max"), CgroupCpuMax = Read("/sys/fs/cgroup/cpu.max"),
            CgroupSwapMax = Read("/sys/fs/cgroup/memory.swap.max"),
            Diagnostics = container != null && inspection == null ? "podman inspect unavailable in this process; supply the host-side snapshot for official runs." : null,
        };
    }
    private static string? Read(string path) => File.Exists(path) ? File.ReadAllText(path).Trim() : null;
    [StructLayout(LayoutKind.Sequential)]
    private struct MemoryStatus
    {
        public uint Length, Load;
        public ulong TotalPhysical, AvailablePhysical, TotalPageFile, AvailablePageFile, TotalVirtual, AvailableVirtual, AvailableExtended;
    }
    [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatus status);
}
