using System;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// Identifier wire parsing and formatting. Guid is deliberately not a baseline: its alphabet,
/// validation and representation differ from ULID's Crockford base32 format.
/// </summary>
public class UlidBenchmarks
{
    private string _text = null!;
    private byte[] _utf8 = null!;
    private Ulid _value;
    private char[] _characters = null!;
    private byte[] _bytes = null!;

    [GlobalSetup]
    public void Setup()
    {
        _text = "01ARZ3NDEKTSV4RRFFQ69G5FAV";
        _utf8 = Encoding.UTF8.GetBytes(_text);
        _value = Ulid.Parse(_text);
        _characters = new char[26];
        _bytes = new byte[26];
        if (_value.ToString() != _text || !FormatCharacters() || !FormatUtf8() || RejectInvalid() ||
            ParseSpan() != _value || ParseUtf8() != _value || new string(_characters) != _text || Encoding.UTF8.GetString(_bytes) != _text)
            throw new InvalidOperationException("ULID round trip failed.");
    }

    [Benchmark] public Ulid Parse() => Ulid.Parse(_text);
    [Benchmark] public Ulid ParseSpan() => Ulid.Parse(_text.AsSpan());
    [Benchmark] public Ulid ParseUtf8() => Ulid.Parse(_utf8.AsSpan());
    [Benchmark] public bool RejectInvalid() => Ulid.TryParse("01ARZ3NDEKTSV4RRFFQ69G5FA!", out _);
    [Benchmark] public string Format() => _value.ToString();
    [Benchmark] public bool FormatCharacters() => _value.TryWriteStringify(_characters.AsSpan());
    [Benchmark] public bool FormatUtf8() => _value.TryWriteStringify(_bytes.AsSpan());
}
