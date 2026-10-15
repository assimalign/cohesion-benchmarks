using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class GraphModel : IBenchmarkModel
{
    public string Id => "graph";
    public DatasetModel DatasetModel => DatasetModel.Graph;
    public IReadOnlyList<OperationDescriptor> Operations { get; } = new OperationDescriptor[]
    {
        new("graph.person-profile", "point read", true, TimeoutClass.Point, 20, 10),
        new("graph.person-recent-messages", "1-hop", true, TimeoutClass.Point, 15, 8),
        new("graph.person-friends", "1-hop", true, TimeoutClass.Point, 15, 8),
        new("graph.friends-recent-posts", "2-hop", true, TimeoutClass.Point, 10, 5),
        new("graph.fof-recent-messages", "3-hop", true, TimeoutClass.Analytics, 8, 4),
        new("graph.friends-within-3-by-name", "variable-length", true, TimeoutClass.Analytics, 5, 3),
        new("graph.shortest-path-knows", "path", true, TimeoutClass.Analytics, 5, 3),
        new("graph.friend-recommendation", "pattern + aggregate", true, TimeoutClass.Analytics, 5, 3),
        new("graph.tag-cooccurrence", "pattern + aggregate", true, TimeoutClass.Analytics, 5, 3),
        new("graph.country-person-count", "aggregate", true, TimeoutClass.Analytics, 2, 3),
        new("graph.add-post", "write", true, TimeoutClass.Point, 4, 20, OperationAccess.Write),
        new("graph.add-like", "write", true, TimeoutClass.Point, 4, 20, OperationAccess.Write),
        new("graph.add-friendship", "write", true, TimeoutClass.Point, 2, 10, OperationAccess.Write)
    };
    public IReadOnlyList<TargetDescriptor> Targets { get; } = new TargetDescriptor[]
    {
        new("neo4j", "Neo4j 5 Community", true, "Bolt", "Neo4j"),
        new("cohesion-graph-embedded", "Cohesion Graph embedded", true, "in-process", "Cohesion Graph"),
        new("cohesion-graph-tcp", "Cohesion Graph TCP", true, "TCP", "Cohesion Graph", "cohesion-graph-embedded")
    };
    public IBenchmarkTarget CreateTarget(string id, TargetOptions options)
    {
        foreach (var descriptor in Targets)
            if (descriptor.Id == id) return id == "neo4j" ? new Neo4jTarget(this, descriptor, options) : new CohesionTarget(this, descriptor, options);
        throw new ArgumentException("Unknown graph target: " + id, nameof(id));
    }
    public IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation, int clientIndex, ParameterStreamContext context)
        => new GraphParameterStream(manifest, seed, operation, clientIndex, context);

    internal Dictionary<string, OperationSupport> Support(bool neo4j, bool tcp)
    {
        var support = new Dictionary<string, OperationSupport>(StringComparer.Ordinal);
        const string language = "resources/Database/Assimalign.Cohesion.Database.Graph.Language/docs/DESIGN.md:98-115";
        foreach (var operation in Operations)
        {
            OperationSupport entry = new(SupportLevel.Native);
            if (!neo4j) entry = operation.Id switch
            {
                "graph.person-profile" or "graph.add-post" or "graph.add-like" or "graph.add-friendship" => entry,
                "graph.shortest-path-knows" => new(SupportLevel.Unsupported,
                    tcp ? "No shortest-path language or traversal wire API." : "TraverseAsync returns nodes without depth or predecessor; one BFS request cannot return shortest-path length. Reconstructing paths in the client is outside DESIGN §6.",
                    "resources/Database/Assimalign.Cohesion.Database.Graph/src/GraphDatabase.cs:368-415; GraphTraversal.cs:11-15"),
                "graph.friend-recommendation" => new(SupportLevel.Unsupported,
                    "GQL cannot exclude existing friends in the engine: NOT EXISTS / negative relationship patterns are absent. Client anti-joins are forbidden.", language),
                "graph.fof-recent-messages" or "graph.friends-within-3-by-name" when tcp => new(SupportLevel.Unsupported,
                    "Multiple fixed-length statements require one transaction; graph wire protocol has no explicit transactions or UNION/quantified paths.",
                    "resources/Database/Assimalign.Cohesion.Database.Graph.Client/src/GraphConnection.cs:13-16"),
                "graph.tag-cooccurrence" or "graph.country-person-count" => new(SupportLevel.Emulated,
                    "Engine selects all paths and predicates; client groups, counts and orders the selected rows. No GQL aggregates or ORDER BY.", language),
                "graph.fof-recent-messages" => new(SupportLevel.Emulated,
                    "One- and two-hop friend patterns in one snapshot transaction; client groups duplicate message ids, sorts and takes 20. Statement bounds are errors, with no reduced-pattern retry.", language),
                "graph.friends-within-3-by-name" => new(SupportLevel.Emulated,
                    "Three fixed-length patterns in one snapshot transaction, name predicate in the engine; client takes minimum distance per id then first 20. Statement bounds are errors.", language),
                _ => new(SupportLevel.Emulated, "Engine selects paths; client sorts and applies the specified top-N. No GQL ORDER BY/LIMIT. Statement bounds are errors.", language)
            };
            support.Add(operation.Id, entry);
        }
        return support;
    }
}
