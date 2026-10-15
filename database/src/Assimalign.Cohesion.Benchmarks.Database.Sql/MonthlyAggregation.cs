using System;
using System.Globalization;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

internal sealed class MonthlyAggregation(int year, bool growth) : IVerificationSink
{
    // Lexicographic binary order; all channel values are part of the schema's dataset contract.
    private static readonly string[] Channels = ["mobile", "partner", "store", "web"];
    private readonly decimal[] sums = new decimal[48];
    private readonly bool[] present = new bool[48];
    public void AddRow(ReadOnlySpan<object?> values)
    {
        int month = ((DateTime)values[0]!).Month - 1;
        int channel = Array.IndexOf(Channels, (string)values[1]!);
        if (channel < 0) throw new InvalidOperationException("Unknown commerce channel.");
        int index = month * 4 + channel;
        sums[index] += Convert.ToDecimal(values[2], CultureInfo.InvariantCulture); present[index] = true;
    }
    internal void Complete(IVerificationSink? sink)
    {
        decimal? previous = null;
        var row = new object?[growth ? 5 : 4];
        for (int m = 0; m < 12; m++)
        {
            decimal total = 0; bool found = false;
            for (int c = 0; c < 4; c++)
            {
                int index = m * 4 + c;
                if (!present[index]) continue;
                found = true; total += sums[index];
                if (!growth && sink != null) { row[0] = year; row[1] = m + 1; row[2] = Channels[c]; row[3] = sums[index]; sink.AddRow(row); }
            }
            if (!growth || !found) continue;
            row[0] = year; row[1] = m + 1; row[2] = total; row[3] = previous; row[4] = previous.HasValue ? total - previous.Value : null;
            sink?.AddRow(row); previous = total;
        }
    }
}
