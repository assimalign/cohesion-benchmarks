using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>HDR integer layout: 1 microsecond to 1 hour, 2,048 sub-buckets (three significant decimal digits).
/// Encoding is dbbench-hdr-v1, not the Java HDR wire format: gzip of little-endian magic, max and count array.</summary>
public sealed class LatencyHistogram
{
    public const long HighestTrackableValue = 3_600_000_000;
    private const int Half = 1024;
    private readonly long[] counts = new long[Index(HighestTrackableValue) + 1];
    public long Count { get; private set; }
    public long Maximum { get; private set; }
    public void Record(long microseconds)
    {
        if (microseconds < 1 || microseconds > HighestTrackableValue) throw new ArgumentOutOfRangeException(nameof(microseconds));
        counts[Index(microseconds)]++;
        Count++;
        Maximum = Math.Max(Maximum, microseconds);
    }
    private static int Index(long value)
    {
        int shift = Math.Max(0, BitOperations.Log2((ulong)value) - 10);
        return (shift * Half) + (int)(value >> shift);
    }
    private static long Upper(int index)
    {
        int shift = Math.Max(0, index / Half - 1);
        int sub = index - shift * Half;
        return ((long)(sub + 1) << shift) - 1;
    }
    public long Percentile(double percentile)
    {
        if (percentile < 0 || percentile > 100 || double.IsNaN(percentile)) throw new ArgumentOutOfRangeException(nameof(percentile));
        if (Count == 0) return 0;
        long rank = Math.Max(1, (long)Math.Ceiling(Count * percentile / 100));
        long seen = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            seen += counts[i];
            if (seen >= rank) return Math.Min(Maximum, Upper(i));
        }
        throw new InvalidDataException("Histogram count is inconsistent.");
    }
    public void Merge(LatencyHistogram other)
    {
        for (int i = 0; i < counts.Length; i++) counts[i] += other.counts[i];
        Count += other.Count;
        Maximum = Math.Max(Maximum, other.Maximum);
    }
    public string Encode()
    {
        using var output = new MemoryStream();
        using (var zip = new GZipStream(output, CompressionLevel.Fastest, true))
        using (var writer = new BinaryWriter(zip))
        {
            writer.Write(0x44424831);
            writer.Write(Maximum);
            writer.Write(counts.Length);
            foreach (long count in counts) writer.Write(count);
        }
        return Convert.ToBase64String(output.ToArray());
    }
    public static LatencyHistogram Decode(string encoded)
    {
        using var input = new MemoryStream(Convert.FromBase64String(encoded));
        using var zip = new GZipStream(input, CompressionMode.Decompress);
        using var reader = new BinaryReader(zip);
        if (reader.ReadInt32() != 0x44424831) throw new InvalidDataException("Unknown histogram encoding.");
        var histogram = new LatencyHistogram { Maximum = reader.ReadInt64() };
        if (histogram.Maximum < 0 || histogram.Maximum > HighestTrackableValue || reader.ReadInt32() != histogram.counts.Length)
            throw new InvalidDataException("Invalid histogram bounds.");
        for (int i = 0; i < histogram.counts.Length; i++)
        {
            long count = reader.ReadInt64();
            if (count < 0) throw new InvalidDataException("Negative histogram count.");
            histogram.counts[i] = count;
            histogram.Count = checked(histogram.Count + count);
        }
        return histogram;
    }
}
