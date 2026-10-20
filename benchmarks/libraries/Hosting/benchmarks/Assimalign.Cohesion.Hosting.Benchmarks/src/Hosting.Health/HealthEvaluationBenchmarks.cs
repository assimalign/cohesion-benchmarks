using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Hosting.Health;
using Assimalign.Cohesion.Hosting.Resources;
using Microsoft.Extensions.DependencyInjection;

using ExtensionsHealth = Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting.Health;

/// <summary>
/// Evaluates named contributors and builds an aggregate report through the public resource control plane and
/// Microsoft health-check service. Both return the same statuses, descriptions, diagnostic data and named entries.
/// Completed checks exclude dependency latency. Microsoft additionally records durations and schedules check tasks;
/// Cohesion evaluates sequentially and inspects diagnostic data for exceptions. Inputs contain no exceptions, so
/// the published values match without failure handling or data replacement. Registration and service-provider
/// construction are outside the measurement.
/// </summary>
public class HealthEvaluationBenchmarks
{
    private ResourceControlPlane _cohesion = null!;
    private ExtensionsHealth.HealthCheckService _extensions = null!;
    private ServiceProvider _services = null!;

    [Params(1, 16)]
    public int ContributorCount { get; set; }

    [Params(false, true)]
    public bool Degraded { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _cohesion = ResourceControlPlane.Create();
        ServiceCollection services = new();
        services.AddLogging();
        IHealthChecksBuilder builder = services.AddHealthChecks();
        for (int index = 0; index < ContributorCount; index++)
        {
            Contributor contributor = new($"component{index}", Degraded && index == ContributorCount - 1);
            _cohesion.AddHealthContributor(contributor);
            builder.Add(new ExtensionsHealth.HealthCheckRegistration(contributor.Name, contributor, null, null));
        }

        _services = services.BuildServiceProvider();
        _extensions = _services.GetRequiredService<ExtensionsHealth.HealthCheckService>();
        ResourceHealthReport cohesion = await Evaluate();
        ExtensionsHealth.HealthReport extensions = await Evaluate_MicrosoftExtensions();
        HealthStatus expected = Degraded ? HealthStatus.Degraded : HealthStatus.Healthy;
        if (cohesion.Status != expected || (int)extensions.Status != (int)expected
            || cohesion.Contributions.Count != ContributorCount || extensions.Entries.Count != ContributorCount)
        {
            throw new InvalidOperationException("The health reports do not contain the expected aggregate.");
        }

        foreach ((string name, HealthContribution contribution) in cohesion.Contributions)
        {
            ExtensionsHealth.HealthReportEntry entry = extensions.Entries[name];
            if ((int)contribution.Status != (int)entry.Status || contribution.Description != entry.Description
                || contribution.Data!["pending"] is not int pending || (int)entry.Data["pending"] != pending)
            {
                throw new InvalidOperationException("The contributors produced different health data.");
            }
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _services.Dispose();

    [Benchmark]
    public ValueTask<ResourceHealthReport> Evaluate() => _cohesion.CheckHealthAsync();

    [Benchmark(Baseline = true)]
    public Task<ExtensionsHealth.HealthReport> Evaluate_MicrosoftExtensions() => _extensions.CheckHealthAsync();

    private sealed class Contributor : IHealthContributor, ExtensionsHealth.IHealthCheck
    {
        private readonly HealthContribution _cohesion;
        private readonly ExtensionsHealth.HealthCheckResult _extensions;

        public Contributor(string name, bool degraded)
        {
            Name = name;
            Dictionary<string, object> data = new() { ["pending"] = degraded ? 10 : 0 };
            string description = degraded ? "backlog" : "ready";
            _cohesion = new HealthContribution(degraded ? HealthStatus.Degraded : HealthStatus.Healthy, description, data);
            _extensions = new ExtensionsHealth.HealthCheckResult(
                degraded ? ExtensionsHealth.HealthStatus.Degraded : ExtensionsHealth.HealthStatus.Healthy,
                description, data: data);
        }

        public string Name { get; }

        public ValueTask<HealthContribution> CheckAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(_cohesion);
        }

        public Task<ExtensionsHealth.HealthCheckResult> CheckHealthAsync(
            ExtensionsHealth.HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_extensions);
        }
    }
}
