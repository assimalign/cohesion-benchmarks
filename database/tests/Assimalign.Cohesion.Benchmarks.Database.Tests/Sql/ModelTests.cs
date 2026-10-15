using System;
using System.Collections.Generic;
using System.Linq;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Benchmarks.Database.Sql;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Sql;

public sealed class ModelTests
{
    [Fact]
    public void Registration_AllTargets_DeclaresEveryOperationAndMixWeight()
    {
        var model = new SqlModel();
        model.Operations.Count.ShouldBe(24);
        model.Operations.Sum(o => o.ReadHeavyWeight).ShouldBe(100);
        model.Operations.Sum(o => o.BalancedWeight).ShouldBe(100);
        model.Operations.Where(o => o.Category.StartsWith("write", StringComparison.Ordinal)).Sum(o => o.ReadHeavyWeight).ShouldBe(10);
        model.Operations.Where(o => o.Category.StartsWith("write", StringComparison.Ordinal)).Sum(o => o.BalancedWeight).ShouldBe(50);
        foreach (TargetDescriptor target in model.Targets)
        {
            var support = model.Supports(target.Id);
            support.Count.ShouldBe(24);
            foreach (OperationSupport entry in support.Values) entry.Validate();
        }
        model.Supports("cohesion-sql-tcp").Values.Count(s => s.Level == SupportLevel.Emulated).ShouldBe(2);
        model.Supports("cohesion-sql-tcp").Values.Count(s => s.Level == SupportLevel.Unsupported).ShouldBe(5);
    }

    [Theory]
    [InlineData(SqlDialect.Cohesion)]
    [InlineData(SqlDialect.PostgreSql)]
    [InlineData(SqlDialect.SqlServer)]
    public void Text_Analytics_UsesHalfOpenRangesAndFractionalAverage(SqlDialect dialect)
    {
        SqlQueries.Text("sql.a.revenue-by-channel", dialect).ShouldContain("order_date >= @start AND order_date < @end");
        SqlQueries.Text("sql.product-review-summary", dialect).ShouldContain("AVG(CAST(rating AS NUMERIC(18,6)))");
        SqlQueries.Text("sql.a.unshipped-orders", dialect).ShouldContain("NOT IN (SELECT order_id FROM shipments)");
    }
}
