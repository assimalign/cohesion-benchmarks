using System.Text.Json.Serialization;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>NativeAOT-compatible manifest metadata, with stable camel-case properties and string model names.</summary>
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, UseStringEnumConverter = true)]
[JsonSerializable(typeof(DatasetManifest))]
public sealed partial class DatasetJsonContext : JsonSerializerContext
{
}
