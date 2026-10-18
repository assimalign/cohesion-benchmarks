using System.Collections.Generic;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// A representative Web resource manifest: two endpoints (one public, TLS), a secret mount, readiness and liveness
/// probes, a control plane, environment variables and a scaled Deployment. The same shape the SDK emits for an
/// ordinary API resource.
/// </summary>
internal static class SampleManifests
{
    public static ResourceManifest CreateWebApi() => new()
    {
        Name = "appa-api",
        Kind = "Web",
        Application = "appa",
        ApplicationModel = "Assimalign.Cohesion.Web.ApplicationModel",
        Artifact = new ResourceManifestArtifact
        {
            Assembly = "Example.AppA.Api",
            Composable = true,
        },
        Endpoints =
        [
            new ResourceManifestEndpoint
            {
                Name = "http",
                Scheme = "http",
                Protocol = "tcp",
                ContainerPort = 8080,
            },
            new ResourceManifestEndpoint
            {
                Name = "https",
                Scheme = "https",
                Protocol = "tcp",
                ContainerPort = 8443,
                Public = true,
                Certificate = "tls",
            },
        ],
        Mounts = [new ResourceManifestMount { Name = "tls", Kind = ResourceMountKind.Secret, ContainerPath = "/cohesion/mounts/tls" }],
        Probes = new ResourceManifestProbes
        {
            Readiness = new ResourceManifestProbe { Endpoint = "https", Http = "/readyz" },
            Liveness = new ResourceManifestProbe { Endpoint = "http", Tcp = true },
        },
        ControlPlane = new ResourceManifestControlPlane
        {
            Endpoint = "http",
            Path = "/cohesion/v1",
        },
        EnvironmentVariables = new Dictionary<string, string>
        {
            ["FEATURE_FLAGS"] = "search,checkout",
            ["LOG_LEVEL"] = "Information",
        },
        Lifecycle = new ResourceManifestLifecycle
        {
            Workload = WorkloadKind.Deployment,
            Replicas = 2,
            MaxReplicas = 4,
            StopGraceSeconds = 30,
        },
        Properties = new Dictionary<string, string>
        {
            ["web.kind"] = "Api",
        },
    };

    public static IApplicationEnvironment Environment { get; } = new BenchmarkEnvironment();

    private sealed class BenchmarkEnvironment : IApplicationEnvironment
    {
        public EnvironmentName Name => AppEnvironment.Keys.Production;

        public bool IsLocal => false;

        public bool IsDevelopment => false;
    }
}
