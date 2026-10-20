using System.IO;
using System.Text;

using Assimalign.Cohesion.Content.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Text;

/// <summary>
/// BOM-free encoding detection plus decoding, mixed-ending line scanning and delimiter tokenization
/// over a small synthetic manifest. UTF-16 input exercises null-pattern detection and actual decoding.
/// No counterpart: StreamReader and string splitting do not retain ending kinds and token positions.
/// </summary>
public class TextParsingBenchmarks
{
    private string _text = null!;
    private MemoryContent _encoded = null!;
    private TextTokenizerOptions _options = null!;

    [GlobalSetup]
    public void Setup()
    {
        StringBuilder text = new();
        for (int index = 0; index < 32; index++)
        {
            text.Append("name: café, enabled: true\r\nitems: [one, two]\n# fin\r");
        }
        _text = text.ToString();
        _encoded = MemoryContent.Create(Encoding.Unicode.GetBytes(_text));
        _options = new TextTokenizerOptions { TokenizeWhitespace = true };
        foreach (string delimiter in new[] { ":", ",", "[", "]", "#" })
        {
            _options.Tokens.Add(new TextTokenDefinition(delimiter));
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _encoded.Dispose();

    [Benchmark]
    public string DecodeDetected()
    {
        using TextContent content = TextContent.Create(_encoded, leaveOpen: true);
        using TextReader reader = content.OpenText();
        return reader.ReadToEnd();
    }

    [Benchmark]
    public int ReadLines()
    {
        using TextLineReader reader = new(new StringReader(_text));
        int length = 0;
        while (reader.TryReadLine(out TextLine line)) { length += line.Text.Length; }
        return length;
    }

    [Benchmark]
    public long Tokenize()
    {
        TextTokenizer tokenizer = new(_text, _options);
        long length = 0;
        while (tokenizer.TryRead(out TextToken token)) { length += token.Value.Length; }
        return length;
    }
}
