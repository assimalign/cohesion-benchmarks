using System;
using System.Numerics;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Eight-round balanced Feistel permutation, cycle-walked onto [1,count]. Expected O(1), no lookup table.</summary>
public sealed class IdPermutation
{
    private readonly ulong count;
    private readonly ulong key;
    private readonly ulong mask;
    private readonly int halfBits;

    public IdPermutation(long count, ulong seed, ulong stream)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
        this.count = (ulong)count;
        halfBits = Math.Max(1, (64 - BitOperations.LeadingZeroCount((ulong)(count - 1)) + 1) / 2);
        mask = (1UL << halfBits) - 1;
        key = CounterRandom.Value(seed, stream, count);
    }

    public long Permute(long id) => Transform(id, false);
    public long Invert(long id) => Transform(id, true);

    private long Transform(long id, bool inverse)
    {
        if (id < 1 || (ulong)id > count) throw new ArgumentOutOfRangeException(nameof(id));
        ulong x = (ulong)(id - 1);
        do
        {
            ulong l = x >> halfBits, r = x & mask;
            for (int j = 0; j < 8; j++)
            {
                int round = inverse ? 7 - j : j;
                ulong f = CounterRandom.Mix(key ^ (inverse ? l : r) ^ ((ulong)round * 0x9e3779b97f4a7c15UL)) & mask;
                (l, r) = inverse ? (r ^ f, l) : (r, l ^ f);
            }
            x = (l << halfBits) | r;
        } while (x >= count);
        return (long)x + 1;
    }
}
