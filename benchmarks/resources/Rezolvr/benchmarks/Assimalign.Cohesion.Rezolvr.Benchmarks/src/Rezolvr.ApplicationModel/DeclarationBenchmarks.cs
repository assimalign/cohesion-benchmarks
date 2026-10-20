using System;
using System.Net;
using Assimalign.Cohesion.ApplicationModel;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Rezolvr.Benchmarks.Rezolvr.ApplicationModel;

/// <summary>
/// Public A/CNAME declaration verbs: DNS name/address/TTL validation, canonical payload serialization
/// and stable command identity generation. Inputs and a fresh descriptor are prepared outside each
/// sample. No model deployment/build or server. Standalone; this is not DNS resolution.
/// </summary>
[InvocationCount(1)]
public class DeclarationBenchmarks
{
    private string[] _names = null!;
    private IPAddress _address = null!;
    private IRezolvrResourceDescriptor _descriptor = null!;

    [Params("A", "CNAME")]
    public string Type { get; set; } = "";

    [Params(1, 64)]
    public int Records { get; set; }

    [GlobalSetup]
    public void CreateInputs()
    {
        _address = IPAddress.Parse("192.0.2.1");
        _names = new string[Records];
        for (int i = 0; i < Records; i++) _names[i] = $"host-{i}.example";
    }

    [IterationSetup]
    public void Setup()
    {
        var builder = Application.CreateBuilder((ApplicationName)"benchmark", []);
        _descriptor = builder.AddRezolvr(new ResourceManifest
        {
            Name = "dns", Kind = "Rezolvr", Application = "benchmark",
            ApplicationModel = "Assimalign.Cohesion.Rezolvr.ApplicationModel",
            Artifact = new ResourceManifestArtifact { Assembly = "Benchmark.Rezolvr", Composable = true },
            Commands = [new ResourceManifestCommand("rezolvr.add-a-record"), new ResourceManifestCommand("rezolvr.add-cname-record")],
            Endpoints =
            [
                new ResourceManifestEndpoint { Name = "dns", Scheme = "dns", Protocol = "udp", ContainerPort = 53 },
                new ResourceManifestEndpoint { Name = "dns-tcp", Scheme = "dns", Protocol = "tcp", ContainerPort = 53 },
                new ResourceManifestEndpoint { Name = "admin", Scheme = "http", Protocol = "tcp", ContainerPort = 8081 }
            ],
            ControlPlane = new ResourceManifestControlPlane { Endpoint = "admin", Path = "/cohesion/v1" },
            Lifecycle = new ResourceManifestLifecycle { Workload = WorkloadKind.Deployment, Replicas = 1 }
        });
    }

    [Benchmark]
    public int DeclareBatch()
    {
        foreach (string name in _names)
        {
            if (Type == "A") _descriptor.AddARecord(name, _address);
            else _descriptor.AddCnameRecord(name, "origin.example");
        }
        return _descriptor.Commands.Count;
    }

    [IterationCleanup]
    public void Verify()
    {
        if (_descriptor.Commands.Count != Records) throw new InvalidOperationException();
        foreach (var command in _descriptor.Commands)
            if (command.Kind != (Type == "A" ? "rezolvr.add-a-record" : "rezolvr.add-cname-record") || command.Payload.IsEmpty)
                throw new InvalidOperationException("Invalid declared command.");
    }
}
