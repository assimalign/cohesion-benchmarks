using System;
using System.Collections.Generic;
using System.IO;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Hosting.Resources;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting.Resources;

/// <summary>
/// Parses frozen invocation environment carriers, resolves endpoints/mounts/settings/references, and establishes an
/// ambient invocation scope. Inputs are private dictionaries, never process environment mutations; no paths are opened
/// and no endpoints are contacted. Microsoft hosting does not implement Cohesion's resource-carrier or mount contract,
/// so substituting a dictionary or raw path lookup would omit validation and fallback behavior and is not compared.
/// </summary>
public class ResourceContextBenchmarks
{
    private Dictionary<string, string?> _environment = null!;
    private ResourceContext _context = null!;
    private ResourceContext _standalone = null!;
    private string[] _names = [];
    private string[] _settingKeys = [];
    private string _fallbackPath = "";

    [Params(4, 32)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _fallbackPath = Path.Combine(AppContext.BaseDirectory, "benchmark-mount.json");
        _environment = new Dictionary<string, string?>
        {
            [AppEnvironment.Variables.Application] = "benchmark",
            [AppEnvironment.Variables.Resource] = "orders",
            [AppEnvironment.Variables.Environment] = "Benchmark",
            [AppEnvironment.Variables.Gateway] = "gateway",
            [AppEnvironment.Variables.ContentRoot] = AppContext.BaseDirectory
        };
        _names = new string[ResourceCount];
        _settingKeys = new string[ResourceCount];
        for (int index = 0; index < ResourceCount; index++)
        {
            string name = $"item{index}";
            _names[index] = name;
            _settingKeys[index] = $"Service:{name}";
            _environment[AppEnvironment.Variables.Endpoint(name, "HOST")] = "127.0.0.1";
            _environment[AppEnvironment.Variables.Endpoint(name, "PORT")] = "5080";
            _environment[AppEnvironment.Variables.Endpoint(name, "SCHEME")] = "http";
            _environment[AppEnvironment.Variables.Mount(name)] = _fallbackPath;
            _environment[AppEnvironment.Variables.Configuration("Service", name)] = "configured";
            _environment[AppEnvironment.Variables.Dependency(name, "http", "URL")] = "http://dependency.test:5080";
        }

        _context = ParseEnvironment();
        _standalone = new ResourceContext(environmentName: "Benchmark", contentRootPath: AppContext.BaseDirectory);
        if (_context.Endpoints.Count != ResourceCount || _context.Mounts.Count != ResourceCount
            || _context.Settings.Count != ResourceCount
            || _context.GetSetting(_settingKeys[0], null) != "configured"
            || _context.GetReference(_names[0], "http").Port != 5080
            || _context.GetMount(_names[0], _fallbackPath).Path != _fallbackPath
            || _standalone.GetEndpoint("http", "http", 5080).Port != 5080)
        {
            throw new InvalidOperationException("The resource fixture did not resolve as expected.");
        }
    }

    [Benchmark]
    public ResourceContext ParseEnvironment() => ResourceContext.FromEnvironment(_environment);

    [Benchmark]
    public int ResolveBindings()
    {
        int checksum = 0;
        for (int index = 0; index < _names.Length; index++)
        {
            string name = _names[index];
            checksum += _context.GetEndpoint(name, "http", null).Port;
            checksum += _context.GetMount(name, _fallbackPath).Path!.Length;
            checksum += _context.GetSetting(_settingKeys[index], null).Length;
            checksum += _context.GetReference(name, "http").Port;
        }

        return checksum;
    }

    [Benchmark]
    public int ResolveFallbacks()
    {
        int checksum = 0;
        foreach (string name in _names)
        {
            checksum += _standalone.GetEndpoint(name, "http", 5080).Port;
            checksum += _standalone.GetMount(name, _fallbackPath).Path!.Length;
            checksum += _standalone.GetSetting(name, "default").Length;
        }

        return checksum;
    }

    [Benchmark]
    public int ScopedResolution()
    {
        using IDisposable scope = ResourceRuntime.CreateScope(_context);
        int checksum = 0;
        foreach (string name in _names)
        {
            checksum += ResourceRuntime.Current.GetEndpoint(name, "http", null).Port;
        }

        return checksum;
    }
}
