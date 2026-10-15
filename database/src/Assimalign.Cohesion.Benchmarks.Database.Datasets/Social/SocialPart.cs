using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

internal sealed class SocialPart : IDisposable
{
    private readonly Dictionary<string,PartOutput> outputs=new(StringComparer.Ordinal);
    internal SocialPart(GenerationSession session,int index,params string[] files)
    {
        foreach(string file in files) outputs.Add(file,session.Csv(file,index,SocialSchema.Header(file)));
    }
    internal CsvWriter this[string file] => outputs[file].Csv;
    public void Dispose() { foreach(PartOutput output in outputs.Values) output.Dispose(); }
}
