using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>JSON persistence using only explicit source-generated type metadata.</summary>
public static class ResultFiles
{
    public static async Task WriteAsync<T>(string path, T value, JsonTypeInfo<T> type, CancellationToken token = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        await using var file = File.Create(path);
        await JsonSerializer.SerializeAsync(file, value, type, token).ConfigureAwait(false);
    }
    public static async Task<T> ReadAsync<T>(string path, JsonTypeInfo<T> type, CancellationToken token = default)
    {
        await using var file = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync(file, type, token).ConfigureAwait(false)
            ?? throw new InvalidDataException("Empty JSON: " + path);
    }
    public static async Task<string> HashAsync(string path, CancellationToken token = default)
    {
        await using var file = File.OpenRead(path);
        return Convert.ToHexStringLower(await SHA256.HashDataAsync(file, token).ConfigureAwait(false));
    }
}
