using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Canonical dataset locations and safe enumeration of the manifest's ordered table parts.</summary>
public static class DatasetFiles
{
    public static string ModelDirectory(DatasetModel model) => model switch
    {
        DatasetModel.Sql => "sql", DatasetModel.Documents => "documents", DatasetModel.Graph => "graph",
        _ => throw new ArgumentOutOfRangeException(nameof(model))
    };

    public static string DirectoryPath(DatasetGenerationOptions options) => Path.GetFullPath(Path.Combine(
        options.OutputDirectory, ModelDirectory(options.Model), "sf" + options.Scale.ToString("0.################", CultureInfo.InvariantCulture)));

    public static IEnumerable<string> EnumerateParts(string directory, DatasetManifest manifest, string tableName)
    {
        string root = Path.GetFullPath(directory) + Path.DirectorySeparatorChar;
        foreach (DatasetTableManifest table in manifest.Tables)
        {
            if (table.Name != tableName) continue;
            foreach (DatasetPartManifest part in table.Parts)
            {
                string path = Path.GetFullPath(Path.Combine(root, part.Path));
                if (Path.IsPathRooted(part.Path) || !path.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Part path escapes the dataset directory.");
                yield return path;
            }
            yield break;
        }
        throw new ArgumentException("No such table in the manifest.", nameof(tableName));
    }
}
