using System;
using System.IO;

using Assimalign.Cohesion.OpenApi.Serialization;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.Fixtures;

internal static class DescriptionFixture
{
    internal static string ReadJson()
    {
        using Stream corpus = typeof(DescriptionFixture).Assembly.GetManifestResourceStream("tictactoe.json")
            ?? throw new InvalidOperationException("The embedded tictactoe.json is missing.");
        using StreamReader reader = new(corpus);
        return reader.ReadToEnd();
    }

    internal static OpenApiDocument ReadDocument() => OpenApiJson.Parse(ReadJson());
}
