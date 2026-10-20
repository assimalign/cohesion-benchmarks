using System;
using System.Text;

using Assimalign.Cohesion.Content.Markdown;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using MarkdigDocument = Markdig.Syntax.MarkdownDocument;
using MarkdigMarkdown = Markdig.Markdown;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Markdown;

/// <summary>
/// Separates delimiter-heavy inline parsing from block parsing, then renders pre-parsed trees.
/// Markdig uses its default CommonMark pipeline. Both inputs stay in the retained shared subset;
/// setup checks identical HTML, including escaping. Render measures neither parser nor fixture creation.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MarkdownShapeBenchmarks
{
    private string _source = null!;
    private MarkdownDocument _document = null!;
    private MarkdigDocument _markdigDocument = null!;

    [Params("Inline", "Blocks")]
    public string Shape { get; set; } = "Inline";

    [GlobalSetup]
    public void Setup()
    {
        string fragment = Shape == "Inline"
            ? "A *small* **parser** with `a < b`, [links](https://example.com), and escaped \\*stars\\*.\n\n"
            : "## Section\n\n> quoted text\n\n- first\n- second\n\n```text\na < b & c\n```\n\n---\n\n";
        StringBuilder source = new();
        for (int index = 0; index < 12; index++) { source.Append(fragment); }
        _source = source.ToString();
        _document = Parse();
        _markdigDocument = Parse_Markdig();
        if (!string.Equals(Render(), Render_Markdig(), StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The shared Markdown subset must produce identical HTML.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Parse")]
    public MarkdownDocument Parse() => MarkdownText.Parse(_source);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Parse")]
    public MarkdigDocument Parse_Markdig() => MarkdigMarkdown.Parse(_source);

    [Benchmark]
    [BenchmarkCategory("Render")]
    public string Render() => MarkdownText.ToHtml(_document);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Render")]
    public string Render_Markdig() => MarkdigMarkdown.ToHtml(_markdigDocument);
}
