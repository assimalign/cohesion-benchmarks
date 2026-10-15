using System.Collections.Generic;
using MongoDB.Bson;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed class DocumentParameters
{
    internal string Key = "", Collection = "", Customer = "", Category = "", Status = "", Prefix = "", Start = "", End = "", OccurredAt = "";
    internal string[] Keys = [];
    internal decimal Price;
    internal int Delta;
    internal byte[] Json = [], RavenJson = [], RavenQuery = [], RavenPatch = [];
    internal string RavenPath = "", Oql = "";
    internal Dictionary<string, object?> Values = new();
    internal BsonDocument[] MongoPipeline = [];
    internal BsonDocument MongoFilter = new(), MongoUpdate = new(), MongoJson = new();
}
