using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Inclusive thresholds and functional-only unranked measurements.</summary>
public sealed class ReportTests
{
    [Fact]
    public async Task GenerateAsync_NominalBudget_UsesObservedSmokeLimits()
    {
        string path = Directory.CreateTempSubdirectory("dbbench-report-budget-").FullName;
        try
        {
            var result = ResultTests.Result("postgresql", 100) with
            {
                Budget = new(3, 6442450944, 3221225472),
                Environment = new() { ContainerInspectJson = "[{\"HostConfig\":{\"NanoCpus\":3000000000,\"Memory\":2147483648}}]" }
            };
            await ResultFiles.WriteAsync(Path.Combine(path, "result.json"), result, ResultJsonContext.Default.BenchmarkResult);
            await ReportGenerator.GenerateAsync(path, Path.Combine(path, "out"));
            string markdown = await File.ReadAllTextAsync(Path.Combine(path, "out", "sql.md"));
            markdown.ShouldContain("Budget: 3 CPUs / 2147483648 bytes");
            markdown.ShouldNotContain("6442450944");
            markdown.ShouldContain("| postgresql | 3 | 2147483648 | 0 |");
        }
        finally { Directory.Delete(path, true); }
    }
    [Theory]
    [InlineData(1.999, false, false)]
    [InlineData(2, false, true)]
    [InlineData(1.499, true, false)]
    [InlineData(1.5, true, true)]
    public void IsDeficit_ThresholdBoundary_IsInclusive(double ratio, bool load, bool expected)
        => ReportStatistics.IsDeficit(ratio, load).ShouldBe(expected);

    [Fact]
    public async Task GenerateAsync_UnrankedFastEngine_ExcludesItFromRatios()
    {
        string path = Directory.CreateTempSubdirectory("dbbench-report-").FullName;
        try
        {
            var cohesion = ResultTests.Result("cohesion-sql-tcp", 200);
            await Write("c", cohesion); await Write("p", ResultTests.Result("postgresql", 100));
            await Write("u", ResultTests.Result("synthetic-unranked", 1, false));
            var findings = await ReportGenerator.GenerateAsync(path, Path.Combine(path, "out"));
            var performance = findings.Single(f => f.Kind == "performance");
            performance.Ratio.ShouldBe(2); performance.BestTarget.ShouldBe("postgresql");
            string markdown = await File.ReadAllTextAsync(Path.Combine(path, "out", "sql.md"));
            markdown.ShouldContain("synthetic-unranked");
            markdown.ShouldContain("200 [200–200]");
            async Task Write(string name, BenchmarkResult value)
                => await ResultFiles.WriteAsync(Path.Combine(path, name + ".json"), value, ResultJsonContext.Default.BenchmarkResult);
        }
        finally { Directory.Delete(path, true); }
    }

    [Fact]
    public async Task GenerateAsync_VerificationMismatch_BlocksPerformanceAndRecordsFinding()
    {
        string path = Directory.CreateTempSubdirectory("dbbench-report-").FullName;
        try
        {
            var cohesion = ResultTests.Result("cohesion-sql-tcp", 200) with { Verification = ResultTests.Verified("cohesion-sql-tcp", "wrong") };
            await ResultFiles.WriteAsync(Path.Combine(path, "c.json"), cohesion, ResultJsonContext.Default.BenchmarkResult);
            await ResultFiles.WriteAsync(Path.Combine(path, "p.json"), ResultTests.Result("postgresql", 100), ResultJsonContext.Default.BenchmarkResult);
            var findings = await ReportGenerator.GenerateAsync(path, Path.Combine(path, "out"));
            findings.Any(f => f.Kind == "verification").ShouldBeTrue();
            findings.Any(f => f.Kind == "performance").ShouldBeFalse();
        }
        finally { Directory.Delete(path, true); }
    }
}
