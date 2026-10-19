using System.IO;
using System.Text;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionJson = Assimalign.Cohesion.Configuration.Json;
using ExtensionsConfiguration = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks;

/// <summary>
/// A small appsettings-style document shared by the suite (nested sections, a list of values, a list of objects),
/// and the one way each side loads it: its JSON stream provider. Keys avoid <c>.</c>, which Cohesion treats as a
/// path delimiter.
/// </summary>
internal static class SampleSettings
{
    /// <summary>A three-segment key that resolves to a value.</summary>
    public const string ValueKey = "Service:Timeouts:Request";

    /// <summary>The value <see cref="ValueKey"/> resolves to.</summary>
    public const string ExpectedValue = "00:00:30";

    private static readonly byte[] Json = Encoding.UTF8.GetBytes(
        """
        {
          "Logging": {
            "LogLevel": { "Default": "Information", "Microsoft": "Warning", "System": "Warning" }
          },
          "ConnectionStrings": {
            "Primary": "Server=db;Database=orders;Encrypt=true",
            "Cache": "cache:6379"
          },
          "Service": {
            "Name": "orders",
            "Port": "8080",
            "Timeouts": { "Connect": "00:00:05", "Request": "00:00:30", "Idle": "00:02:00" },
            "Features": [ "search", "export", "audit", "billing" ],
            "Endpoints": [
              { "Name": "http", "Url": "http://0.0.0.0:8080" },
              { "Name": "https", "Url": "https://0.0.0.0:8443" }
            ]
          },
          "AllowedHosts": "*"
        }
        """);

    /// <summary>Builds a Cohesion configuration over a fresh read-only stream of the document.</summary>
    public static CohesionConfiguration.IConfiguration BuildCohesion()
    {
        CohesionConfiguration.IConfigurationBuilder builder = new CohesionConfiguration.ConfigurationBuilder();
        return CohesionJson.ConfigurationBuilderExtensions.AddJsonStream(builder, OpenJson()).Build();
    }

    /// <summary>Builds a Microsoft.Extensions.Configuration root over a fresh read-only stream of the document.</summary>
    public static ExtensionsConfiguration.IConfigurationRoot BuildExtensions()
        => ExtensionsConfiguration.JsonConfigurationExtensions
            .AddJsonStream(new ExtensionsConfiguration.ConfigurationBuilder(), OpenJson())
            .Build();

    private static MemoryStream OpenJson() => new(Json, writable: false);
}
