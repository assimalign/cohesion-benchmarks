using System.Collections.Generic;
using System.Text.Json.Serialization;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Source-generated serialization; reflection is not used for results, environment or manifests.</summary>
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(BenchmarkResult))]
[JsonSerializable(typeof(VerificationFile))]
[JsonSerializable(typeof(List<VerificationMismatch>))]
[JsonSerializable(typeof(EnvironmentSnapshot))]
[JsonSerializable(typeof(DatasetManifest))]
[JsonSerializable(typeof(List<Finding>))]
public partial class ResultJsonContext : JsonSerializerContext;
