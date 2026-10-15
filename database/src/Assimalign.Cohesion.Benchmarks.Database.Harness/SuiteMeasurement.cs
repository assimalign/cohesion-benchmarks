using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One repetition, including all requested client counts.</summary>
public sealed record SuiteMeasurement(int Repetition, DateTimeOffset StartedUtc, DateTimeOffset FinishedUtc,
    IReadOnlyList<OperationMeasurement> Operations, IReadOnlyList<ResourceSample> Samples);
