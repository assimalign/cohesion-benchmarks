using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Assimalign.Cohesion.LogSpace.Hosting;

namespace Assimalign.Cohesion.LogSpace.Benchmarks.LogSpace.Hosting;

// Bind to the pinned package, never a source copy. The ingest reader and store have no public
// in-process entry point. Expression adapters are compiled in setup; measured calls do no reflection.
internal static class LogSpaceFixture
{
    private static readonly Assembly Assembly = typeof(LogSpaceApplication).Assembly;
    private static readonly Type Reader = Assembly.GetType("Assimalign.Cohesion.LogSpace.Hosting.Internal.OtlpLogReader", true)!;
    private static readonly Type Store = Assembly.GetType("Assimalign.Cohesion.LogSpace.Hosting.Internal.LogSegmentStore", true)!;
    internal static readonly DateTimeOffset Start = new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    internal static Func<JsonElement, string?, IList> BindReader()
    {
        var root = Expression.Parameter(typeof(JsonElement));
        var resource = Expression.Parameter(typeof(string));
        return Expression.Lambda<Func<JsonElement, string?, IList>>(
            Expression.Convert(Expression.Call(Reader.GetMethod("Read", BindingFlags.Static | BindingFlags.NonPublic)!, root, resource), typeof(IList)),
            root, resource).Compile();
    }

    internal static StoreAccess CreateStore(string directory) => new(Activator.CreateInstance(Store, directory)!);

    internal static byte[] Payload(int count, bool structured)
    {
        var records = Enumerable.Range(0, count).Select(i => new
        {
            timeUnixNano = Nanoseconds(Start.AddSeconds(i)), severityNumber = 9, severityText = "INFO",
            traceId = "0123456789abcdef0123456789abcdef", spanId = "0123456789abcdef",
            body = structured ? (object)new { kvlistValue = new { values = new[]
            {
                new { key = "event", value = (object)new { stringValue = "request completed" } },
                new { key = "tags", value = (object)new { arrayValue = new { values = new object[] { new { stringValue = "api" }, new { intValue = "200" } } } } }
            } } } : new { stringValue = "GET /orders completed successfully" },
            attributes = new[]
            {
                new { key = "http.status", value = (object)new { intValue = "200" } },
                new { key = "cached", value = (object)new { boolValue = false } },
                new { key = "duration", value = (object)new { doubleValue = 1.25 } }
            }
        }).ToArray();
        return JsonSerializer.SerializeToUtf8Bytes(new { resourceLogs = new[] { new
        {
            resource = new { attributes = new[] { new { key = "service.name", value = new { stringValue = "service-a" } } } },
            scopeLogs = new[] { new { scope = new { name = "Orders" }, logRecords = records } }
        } } });
    }

    internal static string Nanoseconds(DateTimeOffset time) => ((time.UtcTicks - DateTimeOffset.UnixEpoch.UtcTicks) * 100).ToString(System.Globalization.CultureInfo.InvariantCulture);

    internal sealed class StoreAccess
    {
        internal readonly Func<object, bool> Enqueue;
        internal readonly Action<bool> Flush;
        internal readonly Func<string?, DateTimeOffset?, int, string?, (string Body, string? Cursor)> Query;

        internal StoreAccess(object store)
        {
            var method = Store.GetMethod("TryEnqueue", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var record = Expression.Parameter(typeof(object));
            Enqueue = Expression.Lambda<Func<object, bool>>(Expression.Call(Expression.Constant(store), method,
                Expression.Convert(record, method.GetParameters()[0].ParameterType)), record).Compile();
            Flush = Store.GetMethod("Flush", BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate<Action<bool>>(store);
            Query = Store.GetMethod("Query", BindingFlags.Instance | BindingFlags.NonPublic)!
                .CreateDelegate<Func<string?, DateTimeOffset?, int, string?, (string, string?)>>(store);
        }
    }
}
