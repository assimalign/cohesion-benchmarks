using System;
using System.Globalization;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Resource budget attached to a run; observed container limits are captured independently.</summary>
public sealed record ResourceBudget(double Cpus = 0, long MemoryBytes = 0, long DriverMemoryBytes = 0)
{
    /// <summary>Records actual limits for development as well as official runs. Zero means unbounded or unobserved.</summary>
    public static ResourceBudget Observe(EnvironmentSnapshot snapshot, bool embedded)
    {
        long driver = long.TryParse(snapshot.CgroupMemoryMax, CultureInfo.InvariantCulture, out long limit) && limit > 0 ? limit : 0;
        if (embedded)
        {
            string[] cpu = (snapshot.CgroupCpuMax ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double cpus = cpu.Length == 2 && double.TryParse(cpu[0], CultureInfo.InvariantCulture, out double quota) &&
                double.TryParse(cpu[1], CultureInfo.InvariantCulture, out double period) && period > 0 ? quota / period : 0;
            return new(cpus, driver, driver);
        }
        if (snapshot.ContainerInspectJson == null) return new(0, 0, driver);
        using var json = JsonDocument.Parse(snapshot.ContainerInspectJson);
        var config = json.RootElement[0].GetProperty("HostConfig");
        return new(config.GetProperty("NanoCpus").GetInt64() / 1_000_000_000d, config.GetProperty("Memory").GetInt64(), driver);
    }
}
