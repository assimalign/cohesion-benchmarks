using System;

using Assimalign.Cohesion.Content.Markdown;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using MarkdigDocument = Markdig.Syntax.MarkdownDocument;
using MarkdigMarkdown = Markdig.Markdown;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Markdown;

/// <summary>
/// Parsing a README-sized document, and parsing plus rendering it to HTML: what a static content engine or a
/// documentation endpoint pays per page. The baseline is Markdig with its default (CommonMark) pipeline. The input
/// uses only constructs inside Cohesion's retained CommonMark subset (ATX headings, emphasis, code spans and
/// fences, links, images, lists, block quotes, thematic breaks), so both libraries do the same work.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MarkdownBenchmarks
{
    private const string parseCategory = "Parse";
    private const string toHtmlCategory = "ToHtml";

    private const string source = """
        # Cohesion

        A **code-first**, multi-service application framework for .NET. It hosts services *in-process* or
        *out-of-process* from the same `Program.cs`, with the [developer guide](https://example.com/docs/guide)
        covering both.

        ## Getting started

        1. Install the SDK with `dotnet new install Assimalign.Cohesion.Templates`.
        2. Create an application:

           ```shell
           dotnet new cohesion-web -n Storefront
           cd Storefront
           ```

        3. Run it with `dotnet run` and open <https://localhost:5001>.

        > **Note:** the SDK targets `net10.0`. Older target frameworks are not supported, and *NativeAOT*
        > publishing is the default for release builds.

        ## Features

        - Resource areas for web, database, scheduling and messaging
          - Each area ships its own SDK and shared framework
          - Areas compose through one application model
        - A dependency-free **document model** for Markdown, YAML and binary containers
        - Code-first OpenAPI with `3.0`, `3.1` and `3.2` output

        ---

        ### Configuration

        Settings are read from `cohesion.config` and environment variables. A value set in the environment
        *overrides* the file; see [configuration](https://example.com/docs/configuration "Configuration") for
        the precedence rules and ![a diagram](https://example.com/images/precedence.png) of the merge order.

        ```csharp
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRouting();
        await using var app = builder.Build();
        await app.RunAsync();
        ```

        ### Contributing

        Read the **coding standard** before opening a pull request, run `dotnet test` for the area you
        changed, and keep each commit focused on *one* concern.

        """;

    [GlobalSetup]
    public void Setup()
    {
        if (ToHtml().Length == 0 || ToHtml_Markdig().Length == 0)
        {
            throw new InvalidOperationException("The Markdown input rendered no HTML.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(parseCategory)]
    public MarkdigDocument Parse_Markdig() => MarkdigMarkdown.Parse(source);

    [Benchmark]
    [BenchmarkCategory(parseCategory)]
    public MarkdownDocument Parse() => MarkdownText.Parse(source);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(toHtmlCategory)]
    public string ToHtml_Markdig() => MarkdigMarkdown.ToHtml(source);

    [Benchmark]
    [BenchmarkCategory(toHtmlCategory)]
    public string ToHtml() => MarkdownText.ToHtml(source);
}
