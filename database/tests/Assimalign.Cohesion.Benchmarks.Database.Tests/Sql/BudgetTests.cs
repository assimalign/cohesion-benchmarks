using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Sql;

public sealed class BudgetTests
{
    [Fact]
    public void Observe_NetworkSmoke_ReportsTwoGiBAndUnboundedHostDriver()
    {
        var snapshot = new EnvironmentSnapshot { ContainerInspectJson = "[{\"HostConfig\":{\"NanoCpus\":3000000000,\"Memory\":2147483648}}]" };
        ResourceBudget.Observe(snapshot, false).ShouldBe(new ResourceBudget(3, 2147483648, 0));
    }

    [Fact]
    public void Observe_EmbeddedSmoke_UsesCgroupLimitsForEngineAndDriver()
    {
        var snapshot = new EnvironmentSnapshot { CgroupMemoryMax = "2147483648", CgroupCpuMax = "300000 100000" };
        ResourceBudget.Observe(snapshot, true).ShouldBe(new ResourceBudget(3, 2147483648, 2147483648));
    }
}
