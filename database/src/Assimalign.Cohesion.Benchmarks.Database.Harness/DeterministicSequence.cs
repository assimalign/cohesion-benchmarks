using System.Text;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Stable FNV-1a operation key plus SplitMix64; independent of runtime string hashing and task scheduling.</summary>
public sealed class DeterministicSequence
{
    private ulong state;
    public DeterministicSequence(ulong seed, string operation, int clientIndex)
    {
        ulong key = 14695981039346656037;
        foreach (byte b in Encoding.UTF8.GetBytes(operation)) key = unchecked((key ^ b) * 1099511628211);
        state = seed ^ key ^ unchecked((ulong)clientIndex * 0xD6E8FEB86659FD93UL);
    }
    public ulong Next()
    {
        ulong z = unchecked(state += 0x9E3779B97F4A7C15UL);
        z = unchecked((z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL);
        z = unchecked((z ^ (z >> 27)) * 0x94D049BB133111EBUL);
        return z ^ (z >> 31);
    }
}
