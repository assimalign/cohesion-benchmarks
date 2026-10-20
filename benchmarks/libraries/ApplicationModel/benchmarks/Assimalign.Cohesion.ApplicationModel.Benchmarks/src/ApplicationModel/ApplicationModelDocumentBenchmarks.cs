using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// Export/describe document creation, validated parsing and graph reconstruction. These enforce
/// Cohesion's manifest/plan/graph contract, so plain JSON serialization is not a fair counterpart.
/// </summary>
public class ApplicationModelDocumentBenchmarks
{
    private IApplicationModel _model = null!;
    private ApplicationModelDocument _document = null!;
    private string _json = null!;

    [Params(4, 24)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        using var diagnostics = new PlannerDiagnostics();
        _model = new ApplicationGraph(ResourceCount, "orders").CreateBuilder(new CompositionGateway()).Build().Model;
        _document = ApplicationModelDocument.Create(_model);
        using var stream = new MemoryStream();
        _document.Save(stream);
        _json = Encoding.UTF8.GetString(stream.ToArray());
    }

    [Benchmark]
    public ApplicationModelDocument CreateDocument() => ApplicationModelDocument.Create(_model);

    [Benchmark]
    public ApplicationModelDocument ParseDocument() => ApplicationModelDocument.Parse(_json);

    [Benchmark]
    public IApplicationModel ReconstructModel() => _document.ToModel();
}
