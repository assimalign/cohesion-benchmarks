using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNetProtection = Microsoft.AspNetCore.DataProtection;
using CohesionProtection = Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.Security.Benchmarks.Security.DataProtection;

/// <summary>
/// Protect and unprotect through a purpose-bound protector, the call antiforgery tokens, auth cookies and sessions
/// make once per request. Both sides use an in-memory key ring, so only the per-call work is measured: Cohesion
/// derives an HKDF subkey and creates an AES-GCM instance on every call; ASP.NET Core Data Protection (the baseline,
/// AES-256-CBC + HMAC-SHA256 by default) derives per-payload subkeys from a random key modifier. Two payload sizes
/// separate the fixed per-call cost from the per-byte cost.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class DataProtectorBenchmarks
{
    private const string Purpose = "Cohesion.Benchmarks.Antiforgery";

    private CohesionProtection.IDataProtector _cohesion = null!;
    private AspNetProtection.IDataProtector _aspNet = null!;
    private byte[] _plaintext = null!;
    private byte[] _cohesionProtected = null!;
    private byte[] _aspNetProtected = null!;

    [Params(64, 1024)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _plaintext = RandomNumberGenerator.GetBytes(PayloadSize);

        _cohesion = CohesionProtection.DataProtectionProvider
            .Create(new InMemoryKeyRepository(), static options => options.ApplicationDiscriminator = "benchmarks")
            .CreateProtector(Purpose);
        _aspNet = AspNetProtection.DataProtectionCommonExtensions.CreateProtector(
            new AspNetProtection.EphemeralDataProtectionProvider(), Purpose);

        // Creates the active key outside the measurement and gives Unprotect a valid payload.
        _cohesionProtected = _cohesion.Protect(_plaintext);
        _aspNetProtected = _aspNet.Protect(_plaintext);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Protect")]
    public byte[] Protect_AspNetCore() => _aspNet.Protect(_plaintext);

    [Benchmark]
    [BenchmarkCategory("Protect")]
    public byte[] Protect() => _cohesion.Protect(_plaintext);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Unprotect")]
    public byte[] Unprotect_AspNetCore() => _aspNet.Unprotect(_aspNetProtected);

    [Benchmark]
    [BenchmarkCategory("Unprotect")]
    public byte[] Unprotect() => _cohesion.Unprotect(_cohesionProtected);

    /// <summary>The key ring's backing store, kept in memory so no file I/O enters the measurement.</summary>
    private sealed class InMemoryKeyRepository : CohesionProtection.IKeyRepository
    {
        private readonly List<CohesionProtection.KeyDocument> _keys = [];

        public IReadOnlyList<CohesionProtection.KeyDocument> GetAllKeys()
        {
            lock (_keys)
            {
                return _keys.ToArray();
            }
        }

        public void StoreKey(CohesionProtection.KeyDocument key)
        {
            lock (_keys)
            {
                _keys.Add(key);
            }
        }
    }
}
