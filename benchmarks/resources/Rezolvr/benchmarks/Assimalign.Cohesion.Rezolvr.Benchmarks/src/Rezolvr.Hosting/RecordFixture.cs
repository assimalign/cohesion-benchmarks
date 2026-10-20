using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Hosting.Resources;
using Assimalign.Cohesion.Rezolvr.Hosting;

namespace Assimalign.Cohesion.Rezolvr.Benchmarks.Rezolvr.Hosting;

// Internal repository and handler are the implementation under test, not substitutes. Bind once
// in setup: the public host otherwise requires an HTTP control-plane listener to reach them.
internal static class RecordFixture
{
    private static readonly Assembly Assembly = typeof(RezolvrApplication).Assembly;
    private static readonly Type Repository = Assembly.GetType("Assimalign.Cohesion.Rezolvr.Hosting.Internal.RezolvrRecordRepository", true)!;
    private static readonly Type Handler = Assembly.GetType("Assimalign.Cohesion.Rezolvr.Hosting.Internal.RezolvrResourceCommandHandler", true)!;

    internal static string Kind(string type) => type == "A" ? "rezolvr.add-a-record" : "rezolvr.add-cname-record";

    internal static ResourceCommand Command(string type, int index)
    {
        string name = $"host-{index}.example";
        byte[] payload = type == "A"
            ? JsonSerializer.SerializeToUtf8Bytes(new { name, address = "192.0.2.1", ttlSeconds = 300 })
            : JsonSerializer.SerializeToUtf8Bytes(new { name, target = "origin.example", ttlSeconds = 300 });
        return new ResourceCommand($"record-{index}", Kind(type), "benchmark", name, payload);
    }

    internal static void Seed(string directory, string type, int count)
    {
        Directory.CreateDirectory(directory);
        var records = Enumerable.Range(0, count).Select(i =>
        {
            var command = Command(type, i);
            return new { id = command.Id, kind = command.Kind, owner = command.Owner, key = command.Key,
                payload = command.Payload.ToArray(), value = type == "A" ? "192.0.2.1" : "origin.example", ttlSeconds = 300 };
        }).ToArray();
        File.WriteAllBytes(Path.Combine(directory, "records.json"), JsonSerializer.SerializeToUtf8Bytes(records));
    }

    internal static Access Open(string directory, string type) => new(Activator.CreateInstance(Repository, directory)!, type);

    internal sealed class Access
    {
        internal readonly Func<CancellationToken, Task<IReadOnlyList<ResourceCommand>>> Read;
        internal readonly IResourceCommandHandler Handler;
        internal Access(object repository, string type)
        {
            Read = Repository.GetMethod("ReadCommandsAsync", BindingFlags.Instance | BindingFlags.NonPublic)!
                .CreateDelegate<Func<CancellationToken, Task<IReadOnlyList<ResourceCommand>>>>(repository);
            Handler = (IResourceCommandHandler)Activator.CreateInstance(RecordFixture.Handler, Kind(type), repository)!;
        }
    }
}
