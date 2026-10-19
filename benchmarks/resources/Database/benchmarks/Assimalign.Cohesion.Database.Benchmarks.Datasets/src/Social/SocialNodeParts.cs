using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

// Node rows contain long text. Split them inside aligned, larger relationship ranges, without rereading data.
internal sealed class SocialNodeParts(GenerationSession session,string file,int perPart) : IDisposable
{
    private PartOutput? output;
    private int current=-1;
    internal CsvWriter Writer(long id)
    {
        int index=checked((int)((id-1)/perPart));
        if(index!=current)
        {
            output?.Dispose();
            output=session.Csv(file,index,SocialSchema.Header(file));
            current=index;
        }
        return output!.Csv;
    }
    public void Dispose() => output?.Dispose();
}
