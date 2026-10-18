using System;
using System.IO;

using Assimalign.Cohesion.Content.Yaml;

using BenchmarkDotNet.Attributes;

using YamlDotNetStream = YamlDotNet.RepresentationModel.YamlStream;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Yaml;

/// <summary>
/// Parsing a configuration-sized YAML document into a node tree: how application configuration, manifests and
/// YAML OpenAPI descriptions are read. The baseline is YamlDotNet's representation model
/// (<c>YamlStream.Load</c>), the equivalent document-tree API.
/// </summary>
public class YamlBenchmarks
{
    private const string source = """
        # Service manifest
        name: storefront
        version: 2.4.1
        enabled: true
        replicas: 3
        ratio: 0.75
        owner: null
        description: |
          The storefront service renders the catalog and checkout pages.
          It talks to the inventory and payment services over HTTP.
        labels:
          tier: frontend
          team: "commerce-web"
          region: 'us-east-2'
        defaults: &defaults
          timeout: 30s
          retries: 3
          backoff: exponential
        endpoints:
          - name: catalog
            path: /api/catalog
            methods: [GET, HEAD]
            settings: *defaults
          - name: checkout
            path: /api/checkout
            methods: [POST]
            settings:
              timeout: 10s
              retries: 0
              backoff: none
          - name: health
            path: /healthz
            methods: [GET]
            settings: *defaults
        dependencies:
          inventory:
            url: https://inventory.internal:8443
            pool: { min: 2, max: 32 }
          payments:
            url: https://payments.internal:8443
            pool: { min: 1, max: 8 }
        features:
          - recommendations
          - "gift-cards"
          - saved-carts
        """;

    [GlobalSetup]
    public void Setup()
    {
        if (Parse().Documents.Count != 1 || Parse_YamlDotNet().Documents.Count != 1)
        {
            throw new InvalidOperationException("The YAML input did not parse to one document.");
        }
    }

    [Benchmark(Baseline = true)]
    public YamlDotNetStream Parse_YamlDotNet()
    {
        YamlDotNetStream stream = new();
        stream.Load(new StringReader(source));
        return stream;
    }

    [Benchmark]
    public YamlStream Parse() => YamlText.Parse(source);
}
