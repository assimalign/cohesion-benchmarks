using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Stateless keyed counters using the SplitMix64 finalizer. Stream names are stable UTF-16 hashes.</summary>
public static class CounterRandom
{
    public static ulong Mix(ulong value)
    {
        value = (value ^ (value >> 30)) * 0xbf58476d1ce4e5b9UL;
        value = (value ^ (value >> 27)) * 0x94d049bb133111ebUL;
        return value ^ (value >> 31);
    }

    public static ulong Stream(string name)
    {
        ulong hash = 14695981039346656037UL;
        foreach (char ch in name) hash = (hash ^ ch) * 1099511628211UL;
        return Mix(hash);
    }

    public static ulong Value(ulong seed, ulong stream, long index, ulong attribute = 0) =>
        Mix(Mix(seed ^ 0x9e3779b97f4a7c15UL) ^ Mix(stream) ^ Mix((ulong)index) ^ Mix(attribute + 0x632be59bd9b4e019UL));
}
