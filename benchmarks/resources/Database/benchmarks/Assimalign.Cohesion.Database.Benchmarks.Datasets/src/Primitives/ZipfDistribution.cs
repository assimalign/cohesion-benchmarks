using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Bounded Zipf using Hörmann–Derflinger rejection-inversion, with constant memory and expected O(1) work.</summary>
/// <remarks>Integral of x^-s is evaluated with expm1/log1p for stability near s=1. No CDF table is built.</remarks>
public sealed class ZipfDistribution
{
    private readonly long count;
    private readonly double exponent;
    private readonly double lower;
    private readonly double upper;

    public ZipfDistribution(long count, double exponent = 1.08)
    {
        if (count < 1 || count > 1L << 50) throw new ArgumentOutOfRangeException(nameof(count));
        if (!double.IsFinite(exponent) || exponent < 0 || exponent > 8) throw new ArgumentOutOfRangeException(nameof(exponent));
        this.count = count;
        this.exponent = exponent;
        lower = Integral(1.5) - 1;
        upper = Integral(count + 0.5);
    }

    private double Integral(double x) => Math.Abs(1 - exponent) < 1e-12
        ? Math.Log(x) : double.ExpM1((1 - exponent) * Math.Log(x)) / (1 - exponent);

    private double Inverse(double x) => Math.Abs(1 - exponent) < 1e-12
        ? Math.Exp(x) : Math.Exp(double.LogP1((1 - exponent) * x) / (1 - exponent));

    public long Sample(ref EntityRandom random)
    {
        while (true)
        {
            double u = upper + random.NextDouble() * (lower - upper);
            long k = Math.Clamp((long)Math.Floor(Inverse(u) + 0.5), 1, count);
            if (u >= Integral(k + 0.5) - Math.Pow(k, -exponent)) return k;
        }
    }
}
