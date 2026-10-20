using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Assimalign.Cohesion.Content.Yaml;

using BenchmarkDotNet.Attributes;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Yaml;

/// <summary>
/// Materializes syntax events for flow collections versus block scalars, before composing a node
/// tree. YamlDotNet's streaming parser is collected into a list too: returning its cursor alone would
/// do less work. Both sides retain ordered events and source positions, with no object deserialization.
/// </summary>
public class YamlEventBenchmarks
{
    private string _source = null!;

    [Params("Flow", "Scalars")]
    public string Shape { get; set; } = "Flow";

    [GlobalSetup]
    public void Setup()
    {
        StringBuilder source = new();
        for (int index = 0; index < 16; index++)
        {
            source.Append("entry").Append(index).Append(Shape == "Flow"
                ? ": {name: 'café', flags: [true, false], value: 42}\n"
                : ":\n  literal: |\n    first line\n    second line\n  folded: >-\n    folded words\n    continue here\n  quoted: \"escaped\\ntext\"\n");
        }
        _source = source.ToString();
        if (ParseEvents().Count != ParseEvents_YamlDotNet().Count)
        {
            throw new InvalidOperationException("Expected equivalent YAML syntax event counts.");
        }
    }

    [Benchmark]
    public IReadOnlyList<YamlEvent> ParseEvents() => YamlText.ParseEvents(_source);

    [Benchmark(Baseline = true)]
    public IReadOnlyList<ParsingEvent> ParseEvents_YamlDotNet()
    {
        using StringReader source = new(_source);
        Parser parser = new(source);
        List<ParsingEvent> events = new();
        while (parser.MoveNext()) { events.Add(parser.Current!); }
        return events;
    }
}
