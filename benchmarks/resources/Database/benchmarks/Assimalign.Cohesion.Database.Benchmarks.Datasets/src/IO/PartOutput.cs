using System;
using System.IO;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

internal sealed class PartOutput : IDisposable
{
    private readonly GenerationSession session;
    private readonly string table, path;
    private readonly int index;
    private readonly HashingWriteStream stream;
    private bool disposed;
    internal CsvWriter Csv { get; } = null!;
    internal JsonLinesWriter Json { get; } = null!;

    internal PartOutput(GenerationSession session, string table, int index, string[]? headers)
    {
        this.session = session; this.table = table; this.index = index;
        path = table + "/part-" + index.ToString("D5", System.Globalization.CultureInfo.InvariantCulture) + (headers is null ? ".jsonl" : ".csv");
        Directory.CreateDirectory(Path.Combine(session.Directory, table));
        stream = new HashingWriteStream(Path.Combine(session.Directory, path));
        if (headers is null) Json = new JsonLinesWriter(stream, true);
        else Csv = new CsvWriter(stream, headers, true);
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        long rows = Csv is not null ? Csv.Rows : Json.Rows;
        Csv?.Dispose(); Json?.Dispose();
        string sha = stream.CompleteHash();
        long bytes = stream.Bytes;
        stream.Dispose();
        session.Add(table, index, new DatasetPartManifest { Path = path, Rows = rows, Bytes = bytes, Sha256 = sha });
    }
}
