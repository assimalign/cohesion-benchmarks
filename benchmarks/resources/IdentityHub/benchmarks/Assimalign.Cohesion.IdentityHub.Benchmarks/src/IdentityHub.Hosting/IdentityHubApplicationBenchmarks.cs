using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.IdentityHub.Hosting;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityHub.Benchmarks.IdentityHub.Hosting;

/// <summary>
/// Public composition and validated Build, including client-secret hashing, declared-audience
/// checks, endpoint-service construction and disposal. No listener is started. The explicit Local
/// environment scope makes endpoint validation independent of the runner's environment. A generic DI
/// container does not perform these identity-specific checks, so this is standalone.
/// </summary>
public class IdentityHubApplicationBenchmarks
{
    private string _dataPath = null!;
    private string[] _arguments = null!;
    private string[] _clientIds = null!;
    private AppEnvironmentScope _scope = null!;

    [Params(1, 16)]
    public int ClientCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _dataPath = Directory.CreateTempSubdirectory("cohesion-identity-benchmarks-").FullName;
        _scope = AppEnvironmentScope.Create(
            new Dictionary<string, string> { [AppEnvironment.Keys.EnvironmentKey] = AppEnvironment.Keys.Local },
            CancellationToken.None);
        _arguments = ["--data", _dataPath, "--endpoint", "https://127.0.0.1:8443"];
        _clientIds = new string[ClientCount];
        for (int index = 0; index < _clientIds.Length; index++)
        {
            _clientIds[index] = $"worker-{index}";
        }
    }

    [Benchmark]
    public async Task<IdentityHubApplicationContext> Build()
    {
        using IDisposable scope = AppEnvironment.Enter(_scope);
        IdentityHubApplicationBuilder builder = IdentityHubApplication.CreateBuilder(_arguments);
        builder.AddAudience("orders").AddAudience("inventory");
        foreach (string clientId in _clientIds)
        {
            builder.AddClient(clientId, static options =>
            {
                options.ClientSecret = "benchmark-only-client-secret";
                options.AllowDeviceAuthorization = true;
                options.Audiences.Add("orders");
                options.Audiences.Add("inventory");
            });
        }

        await using IdentityHubApplication application = builder.Build();
        return application.Context;
    }

    [GlobalCleanup]
    public void Cleanup() => Directory.Delete(_dataPath, recursive: true);
}
