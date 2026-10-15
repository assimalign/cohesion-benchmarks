using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>
/// Looks up the generator for a model.
/// </summary>
public static class DatasetGenerators
{
    /// <summary>
    /// Returns the generator for <paramref name="model"/>.
    /// </summary>
    public static IDatasetGenerator Get(DatasetModel model)
    {
        if (model == DatasetModel.Sql) return new CommerceGenerator();
        throw new NotImplementedException($"The {model} dataset generator is not implemented yet.");
    }
}
