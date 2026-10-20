using System;
using System.IO;
using System.Text;

namespace Assimalign.Cohesion.Configuration.Benchmarks;

/// <summary>Equivalent string-valued settings documents; generation belongs to global setup.</summary>
internal sealed class ProviderSettings
{
    public ProviderSettings(int count)
    {
        StringBuilder json = new("{\"Service\":{");
        StringBuilder ini = new("; Benchmark settings\n[Service]\n");
        StringBuilder xml = new("<settings><Service>");
        Arguments = new string[count];
        for (int index = 0; index < count; index++)
        {
            string key = $"Item{index}";
            string value = $"value{index}";
            if (index != 0)
            {
                json.Append(',');
            }

            json.Append('"').Append(key).Append("\":\"").Append(value).Append('"');
            ini.Append(key).Append('=').Append(value).Append('\n');
            xml.Append('<').Append(key).Append('>').Append(value).Append("</").Append(key).Append('>');
            Arguments[index] = $"--Service:{key}={value}";
        }

        Json = Encoding.UTF8.GetBytes(json.Append("}}").ToString());
        Ini = Encoding.UTF8.GetBytes(ini.ToString());
        Xml = Encoding.UTF8.GetBytes(xml.Append("</Service></settings>").ToString());
        LastKey = $"Service:Item{count - 1}";
        LastValue = $"value{count - 1}";
    }

    public byte[] Json { get; }
    public byte[] Ini { get; }
    public byte[] Xml { get; }
    public string[] Arguments { get; }
    public string LastKey { get; }
    public string LastValue { get; }

    public static MemoryStream Open(byte[] bytes) => new(bytes, writable: false);

    public void Verify(string? cohesion, string? extensions)
    {
        if (cohesion != LastValue || extensions != LastValue)
        {
            throw new InvalidOperationException("The providers did not load the expected nested setting.");
        }
    }
}
