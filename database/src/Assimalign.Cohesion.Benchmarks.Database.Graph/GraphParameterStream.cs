using System;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class GraphParameterStream : IParameterStream
{
    private readonly SocialDescriptor data;
    private readonly DeterministicSequence random;
    private readonly ParameterStreamContext context;
    private readonly string operation;
    private readonly int client;
    private long sequence;

    internal GraphParameterStream(DatasetManifest manifest, ulong seed, string operation, int client, ParameterStreamContext context)
    {
        data = new(manifest); random = new(seed, operation, client); this.operation = operation; this.client = client; this.context = context;
    }
    public object Next()
    {
        long person = Id(data.Persons), other = Id(data.Persons);
        if (other == person) other = other % data.Persons + 1;
        long post = Id(data.Posts), tag = Id(SocialDescriptor.Tags);
        string name = data.FirstName(Id(data.Persons));
        long inserted = context.WriteId(data.IdRange("post").Maximum, client, sequence++);
        return GraphQueries.Create(operation, person, other, post, tag, name, inserted);
    }
    private long Id(long maximum) => 1 + (long)(random.Next() % (ulong)maximum);
}
