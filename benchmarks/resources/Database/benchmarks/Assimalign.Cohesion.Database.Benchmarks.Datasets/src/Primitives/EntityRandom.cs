using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>A local deterministic sequence; advancing one entity cannot affect any other entity.</summary>
public struct EntityRandom
{
    private readonly ulong key;
    private ulong counter;

    public EntityRandom(ulong seed, ulong entityKind, long id)
    {
        key = CounterRandom.Value(seed, entityKind, id);
        counter = 0;
    }

    public EntityRandom(ulong seed, string entityKind, long id) : this(seed, CounterRandom.Stream(entityKind), id) { }

    public ulong NextUInt64() => CounterRandom.Mix(key + (++counter * 0x9e3779b97f4a7c15UL));
    public double NextDouble() => (NextUInt64() >> 11) * (1.0 / 9007199254740992.0);
    public bool Chance(double probability) => NextDouble() < probability;

    /// <summary>Unbiased inclusive bounds, including the full signed Int64 domain.</summary>
    public long Long(long minimum, long maximum)
    {
        if (minimum > maximum) throw new ArgumentOutOfRangeException(nameof(maximum));
        ulong width = unchecked((ulong)maximum - (ulong)minimum + 1);
        if (width == 0) return unchecked((long)NextUInt64());
        ulong threshold = unchecked(0UL - width) % width;
        ulong value;
        do { value = NextUInt64(); } while (value < threshold);
        return unchecked((long)((ulong)minimum + value % width));
    }

    public int Int(int minimum, int maximum) => (int)Long(minimum, maximum);

    /// <summary>Uniform on the fixed-scale decimal lattice within inclusive bounds.</summary>
    public decimal Decimal(decimal minimum, decimal maximum, int scale = 2)
    {
        if (scale < 0 || scale > 9) throw new ArgumentOutOfRangeException(nameof(scale));
        decimal factor = (decimal)Math.Pow(10, scale);
        return Long(checked((long)decimal.Ceiling(minimum * factor)), checked((long)decimal.Floor(maximum * factor))) / factor;
    }

    public double Normal(double mean = 0, double standardDeviation = 1) =>
        mean + standardDeviation * Math.Sqrt(-2 * Math.Log(1 - NextDouble())) * Math.Cos(2 * Math.PI * NextDouble());

    public int LogNormalLength(int minimum, int maximum, double median, double sigma = 0.8) =>
        (int)Math.Clamp(Math.Round(Math.Exp(Normal(Math.Log(median), sigma))), minimum, maximum);
}
