using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

internal sealed class GenerationSession
{
    internal const int Version = 1;
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, DatasetPartManifest>> parts = new(StringComparer.Ordinal);
    private readonly DatasetGenerationOptions options;
    private readonly IProgress<DatasetProgress>? progress;
    internal string Directory { get; }

    internal GenerationSession(DatasetGenerationOptions options, DatasetModel model, IProgress<DatasetProgress>? progress)
    {
        if (options.Model != model) throw new ArgumentException("Generator and model do not match.");
        if (!double.IsFinite(options.Scale) || options.Scale <= 0 || options.Scale > 10000) throw new ArgumentOutOfRangeException(nameof(options.Scale));
        if (options.Threads < 1) throw new ArgumentOutOfRangeException(nameof(options.Threads));
        this.options = options; this.progress = progress;
        Directory = DatasetFiles.DirectoryPath(options);
        if (System.IO.Directory.Exists(Directory))
        {
            if (!options.Overwrite) throw new IOException("Dataset already exists. Set Overwrite to replace it.");
            // The canonical path always has two fixed child components beneath the caller's output root.
            string root = Path.GetFullPath(options.OutputDirectory).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.StartsWith(root, StringComparison.OrdinalIgnoreCase) || Directory == root)
                throw new IOException("Refusing to replace a directory outside the output root.");
            System.IO.Directory.Delete(Directory, true);
        }
        System.IO.Directory.CreateDirectory(Directory);
    }

    internal static long Count(double scale, long perScale) => Math.Max(1, checked((long)Math.Round(scale * perScale, MidpointRounding.AwayFromZero)));

    internal async Task Ranges(long count, int perPart, Action<int, long, long> write, CancellationToken token)
    {
        int number = checked((int)((count + perPart - 1) / perPart));
        var indexes = new int[number];
        for (int i = 0; i < number; i++) indexes[i] = i;
        await Parallel.ForEachAsync(indexes, new ParallelOptions { MaxDegreeOfParallelism = options.Threads, CancellationToken = token },
            (part, ct) => { ct.ThrowIfCancellationRequested(); write(part, (long)part * perPart + 1, Math.Min(count, (long)(part + 1) * perPart)); return ValueTask.CompletedTask; }).ConfigureAwait(false);
    }

    internal PartOutput Csv(string table, int part, string[] headers) => new(this, table, part, headers);
    internal PartOutput Json(string table, int part) => new(this, table, part, null);

    internal void Add(string table, int index, DatasetPartManifest part)
    {
        parts.GetOrAdd(table, _ => new())[index] = part;
        if (progress is not null)
        {
            long rows = 0, bytes = 0;
            foreach (DatasetPartManifest p in parts[table].Values) { rows += p.Rows; bytes += p.Bytes; }
            progress.Report(new DatasetProgress(table, rows, bytes));
        }
    }

    internal DatasetManifest Finish(string name, IReadOnlyList<string> loadOrder, IReadOnlyDictionary<string, long>? maximumIds = null)
    {
        var tables = new List<DatasetTableManifest>();
        foreach (string table in loadOrder)
        {
            var ordered = new SortedDictionary<int, DatasetPartManifest>(parts[table]);
            var list = new List<DatasetPartManifest>(ordered.Values);
            long rows = 0, bytes = 0;
            foreach (DatasetPartManifest part in list) { rows += part.Rows; bytes += part.Bytes; }
            tables.Add(new DatasetTableManifest { Name = table, Rows = rows, Bytes = bytes, Parts = list,
                MaximumId = maximumIds is not null && maximumIds.TryGetValue(table, out long maximum) ? maximum : rows });
        }
        var manifest = new DatasetManifest { GeneratorVersion = Version, Model = options.Model, Name = name,
            Scale = options.Scale, Seed = options.Seed, Tables = tables, Fingerprint = DatasetManifestFile.Fingerprint(tables) };
        DatasetManifestFile.Save(Path.Combine(Directory, "manifest.json"), manifest);
        return manifest;
    }
}
