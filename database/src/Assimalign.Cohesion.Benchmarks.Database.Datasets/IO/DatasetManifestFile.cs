using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Source-generated manifest persistence and ordered part-hash fingerprinting.</summary>
public static class DatasetManifestFile
{
    public static string Fingerprint(IReadOnlyList<DatasetTableManifest> tables)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (DatasetTableManifest table in tables)
            foreach (DatasetPartManifest part in table.Parts) hash.AppendData(Convert.FromHexString(part.Sha256));
        return Convert.ToHexStringLower(hash.GetHashAndReset());
    }

    public static void Save(string path, DatasetManifest manifest)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        JsonSerializer.Serialize(stream, manifest, DatasetJsonContext.Default.DatasetManifest);
        stream.WriteByte((byte)'\n');
    }

    public static DatasetManifest Load(string path)
    {
        using var stream = File.OpenRead(path);
        DatasetManifest manifest = JsonSerializer.Deserialize(stream, DatasetJsonContext.Default.DatasetManifest)
            ?? throw new InvalidDataException("Empty manifest.");
        if (manifest.Fingerprint != Fingerprint(manifest.Tables)) throw new InvalidDataException("Manifest fingerprint is inconsistent.");
        return manifest;
    }
}
