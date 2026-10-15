using System;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Official resource claims are checked against observed limits, including swap.</summary>
public sealed class OfficialRunGuardTests
{
    private static EnvironmentSnapshot Valid(bool embedded) => new()
    {
        BenchmarksCommit = "bench", CohesionBuiltCommits = ["cohesion"],
        Optimization = [new("engine", true, "1+cohesion", null)],
        CgroupMemoryMax = embedded ? "6442450944" : "3221225472",
        CgroupCpuMax = "300000 100000", CgroupSwapMax = "0",
        ContainerInspectJson = """[{"HostConfig":{"Memory":6442450944,"MemorySwap":6442450944,"NanoCpus":3000000000}}]""",
    };
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_ObservedSharedBudget_AcceptsBothAccessPaths(bool embedded)
        => Should.NotThrow(() => OfficialRunGuard.Validate(Valid(embedded), embedded));
    [Fact]
    public void Validate_DevelopmentOrUnobservedLimits_RefusesOfficialLabel()
    {
        Should.Throw<InvalidOperationException>(() => OfficialRunGuard.Validate(Valid(false) with { CgroupMemoryMax = "1073741824" }, false));
        Should.Throw<InvalidOperationException>(() => OfficialRunGuard.Validate(Valid(false) with { ContainerInspectJson = null }, false));
        Should.Throw<InvalidOperationException>(() => OfficialRunGuard.Validate(Valid(true) with { CgroupSwapMax = "max" }, true));
        Should.Throw<InvalidOperationException>(() => OfficialRunGuard.Validate(Valid(true) with { CgroupCpuMax = "max 100000" }, true));
    }
}
