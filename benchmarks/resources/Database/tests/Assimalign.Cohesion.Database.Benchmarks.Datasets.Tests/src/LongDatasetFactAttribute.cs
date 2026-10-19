using System;
using Xunit;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests;

/// <summary>Opt-in expensive generation; enable with DBBENCH_LONG_TESTS=1.</summary>
public sealed class LongDatasetFactAttribute : FactAttribute
{
    public LongDatasetFactAttribute()
    {
        if(Environment.GetEnvironmentVariable("DBBENCH_LONG_TESTS")!="1") Skip="Set DBBENCH_LONG_TESTS=1 to generate and measure SF1 for all models.";
    }
}
