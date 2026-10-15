using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Version and observed settings, never merely an assumed catalog configuration.</summary>
public sealed record EngineDescription(string Version, IReadOnlyDictionary<string, string> EffectiveSettings);
