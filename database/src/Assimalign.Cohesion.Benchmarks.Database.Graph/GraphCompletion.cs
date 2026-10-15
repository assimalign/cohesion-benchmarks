using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class GraphCompletion
{
    internal static void Emit(string operation, List<object?[]> rows, IVerificationSink? sink)
    {
        int limit = int.MaxValue;
        switch (operation)
        {
            case "graph.person-recent-messages": limit = 10; goto case "graph.fof-recent-messages";
            case "graph.friends-recent-posts": limit = 20; goto case "graph.fof-recent-messages";
            case "graph.fof-recent-messages":
                if (limit == int.MaxValue) limit = 20;
                // DISTINCT is an aggregation over engine-selected paths, never a client join/filter.
                var messages = new Dictionary<(string, long), object?[]>();
                foreach (var row in rows) messages.TryAdd(((string)row[0]!, (long)row[1]!), row);
                rows = new(messages.Values);
                rows.Sort(static (a, b) => Compare(Desc(a[2], b[2]), Desc(a[1], b[1]), string.CompareOrdinal((string?)a[0], (string?)b[0])));
                break;
            case "graph.person-friends":
                rows.Sort(static (a, b) => Compare(Desc(a[3], b[3]), Asc(a[0], b[0]), 0)); break;
            case "graph.friends-within-3-by-name":
                var persons = new Dictionary<long, object?[]>();
                foreach (var row in rows)
                    if (!persons.TryGetValue((long)row[0]!, out var previous) || (long)row[3]! < (long)previous[3]!) persons[(long)row[0]!] = row;
                rows = new(persons.Values);
                rows.Sort(static (a, b) => Compare(Asc(a[3], b[3]), Asc(a[0], b[0]), 0)); limit = 20; break;
            case "graph.tag-cooccurrence":
            case "graph.country-person-count":
                var groups = new Dictionary<long, (object? Name, HashSet<long> Members)>();
                foreach (var row in rows)
                {
                    long key = (long)row[0]!;
                    if (!groups.TryGetValue(key, out var group)) { group = (row[1], new()); groups.Add(key, group); }
                    group.Members.Add((long)row[2]!);
                }
                rows = new(groups.Count);
                foreach (var group in groups) rows.Add([group.Key, group.Value.Name, (long)group.Value.Members.Count]);
                if (operation == "graph.tag-cooccurrence") rows.Sort(static (a, b) => Compare(Desc(a[2], b[2]), Asc(a[0], b[0]), 0));
                else rows.Sort(static (a, b) => Asc(a[0], b[0]));
                break;
        }
        if (sink is not null) for (int i = 0; i < rows.Count && i < limit; i++) sink.AddRow(rows[i]);
    }
    private static int Asc(object? a, object? b) => ((long)a!).CompareTo((long)b!);
    private static int Desc(object? a, object? b) => ((long)b!).CompareTo((long)a!);
    private static int Compare(int first, int second, int third) => first != 0 ? first : second != 0 ? second : third;
}
