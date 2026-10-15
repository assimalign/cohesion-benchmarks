using System;
using System.Globalization;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Refuses official labels when observed cgroup/container limits differ from the shared budget.</summary>
public static class OfficialRunGuard
{
    public static void Validate(EnvironmentSnapshot snapshot, bool embedded)
    {
        OptimizationGuard.RequireOptimized(snapshot.Optimization);
        if (snapshot.CohesionBuiltCommits.Count == 0 || string.IsNullOrEmpty(snapshot.BenchmarksCommit))
            throw new InvalidOperationException("Official runs require actual build and benchmarks commit provenance.");
        long driverMemory = (embedded ? 6L : 3L) * 1024 * 1024 * 1024;
        if (!long.TryParse(snapshot.CgroupMemoryMax, CultureInfo.InvariantCulture, out long memory) || memory != driverMemory)
            throw new InvalidOperationException("Official driver memory limit does not match its access path budget.");
        string[] cpu = (snapshot.CgroupCpuMax ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (cpu.Length != 2 || !double.TryParse(cpu[0], CultureInfo.InvariantCulture, out double quota) ||
            !double.TryParse(cpu[1], CultureInfo.InvariantCulture, out double period) || period <= 0 || quota / period != 3)
            throw new InvalidOperationException("Official driver CPU limit must be 3 CPUs.");
        if (snapshot.CgroupSwapMax != "0") throw new InvalidOperationException("Official driver swap must be disabled.");
        if (!embedded)
        {
            if (snapshot.ContainerInspectJson == null) throw new InvalidOperationException("Official network target requires podman inspect evidence (--environment).");
            using var json = JsonDocument.Parse(snapshot.ContainerInspectJson);
            var config = json.RootElement[0].GetProperty("HostConfig");
            const long engineMemory = 6L * 1024 * 1024 * 1024;
            if (config.GetProperty("Memory").GetInt64() != engineMemory || config.GetProperty("MemorySwap").GetInt64() != engineMemory ||
                config.GetProperty("NanoCpus").GetInt64() != 3_000_000_000)
                throw new InvalidOperationException("Official engine requires 3 CPUs, 6 GiB and no swap.");
        }
    }
}
