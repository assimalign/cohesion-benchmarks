using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNetProtection = Microsoft.AspNetCore.DataProtection;
using CohesionProtection = Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.Security.Benchmarks.Security.DataProtection;

/// <summary>
/// Builds purpose chains and processes a request with a dynamically scoped protector. Both stacks provide
/// authenticated, purpose-isolated protection using in-memory keys, with their default algorithms (Cohesion
/// AES-GCM/HKDF-SHA256; ASP.NET Core AES-CBC/HMACSHA256). Cohesion derives and zeroes a fresh subkey on every
/// protect and unprotect: this public path includes key derivation, which has no standalone public API.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PurposeChainBenchmarks
{
    private CohesionProtection.IDataProtectionProvider _cohesion = null!;
    private AspNetProtection.IDataProtectionProvider _aspNet = null!;
    private byte[] _plaintext = null!;
    private string[] _purposes = null!;

    [Params(1, 4)]
    public int Depth { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _plaintext = Enumerable.Range(0, 64).Select(static index => (byte)index).ToArray();
        _purposes = new[] { "orders", "tenant-contoso", "session", "user-42" }.Take(Depth).ToArray();
        _cohesion = CohesionProtection.DataProtectionProvider.Create(new InMemoryKeyRepository(),
            static options => options.ApplicationDiscriminator = "benchmarks");
        _aspNet = new AspNetProtection.EphemeralDataProtectionProvider();
        // Warm the rings and prove independently constructed chains resolve the same purpose-bound key.
        byte[] cohesionPayload = CreateChain().Protect(_plaintext);
        byte[] aspNetPayload = CreateChain_AspNetCore().Protect(_plaintext);
        if (!CreateChain().Unprotect(cohesionPayload).SequenceEqual(_plaintext) ||
            !CreateChain_AspNetCore().Unprotect(aspNetPayload).SequenceEqual(_plaintext))
        {
            throw new InvalidOperationException("Purpose chains must round trip across protector instances.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("CreateChain")]
    public CohesionProtection.IDataProtector CreateChain()
    {
        var protector = _cohesion.CreateProtector(_purposes[0]);
        for (int index = 1; index < _purposes.Length; index++)
        {
            protector = protector.CreateProtector(_purposes[index]);
        }
        return protector;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CreateChain")]
    public AspNetProtection.IDataProtector CreateChain_AspNetCore()
    {
        var protector = _aspNet.CreateProtector(_purposes[0]);
        for (int index = 1; index < _purposes.Length; index++)
        {
            protector = protector.CreateProtector(_purposes[index]);
        }
        return protector;
    }

    [Benchmark]
    [BenchmarkCategory("CreateAndRoundTrip")]
    public byte[] CreateAndRoundTrip()
    {
        var protector = CreateChain();
        return protector.Unprotect(protector.Protect(_plaintext));
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CreateAndRoundTrip")]
    public byte[] CreateAndRoundTrip_AspNetCore()
    {
        var protector = CreateChain_AspNetCore();
        return protector.Unprotect(protector.Protect(_plaintext));
    }

    private sealed class InMemoryKeyRepository : CohesionProtection.IKeyRepository
    {
        private readonly List<CohesionProtection.KeyDocument> _keys = [];

        public IReadOnlyList<CohesionProtection.KeyDocument> GetAllKeys() => _keys.ToArray();

        public void StoreKey(CohesionProtection.KeyDocument key) => _keys.Add(key);
    }
}
