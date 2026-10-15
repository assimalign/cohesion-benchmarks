using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class GraphData
{
    internal const long SamplePersons = 100_000;
    internal const long SamplePosts = 100_000;

    internal static IEnumerable<Dictionary<string, object?>> Nodes(DatasetManifest manifest, string directory, SchemaNode node, long maximum, CancellationToken token)
    {
        foreach (string part in DatasetFiles.EnumerateParts(directory, manifest, node.File))
        {
            using var reader = new CsvReader(File.OpenRead(part));
            while (reader.Read())
            {
                token.ThrowIfCancellationRequested();
                var properties = GraphMapping.ReadProperties(reader, node.Properties);
                if ((long)properties["id"]! <= maximum) yield return properties;
            }
        }
    }

    internal static IEnumerable<Edge> Edges(DatasetManifest manifest, string directory, SchemaRelationship relation, bool sample, CancellationToken token)
    {
        foreach (string part in DatasetFiles.EnumerateParts(directory, manifest, relation.File))
        {
            using var reader = new CsvReader(File.OpenRead(part));
            while (reader.Read())
            {
                token.ThrowIfCancellationRequested();
                long source = long.Parse(reader.GetField(0), CultureInfo.InvariantCulture);
                long target = long.Parse(reader.GetField(1), CultureInfo.InvariantCulture);
                if (sample && (relation.Type == "KNOWS" ? source > SamplePersons && target > SamplePersons : source > SamplePosts)) continue;
                yield return new(source, target, GraphMapping.ReadProperties(reader, relation.Properties, 2));
            }
        }
    }

    internal static bool SampleRelationship(SchemaRelationship r) => r.File is "person_knows_person" or "post_hascreator_person" or "post_hastag_tag";
    internal sealed record Edge(long Source, long Target, Dictionary<string, object?> Properties);
}
