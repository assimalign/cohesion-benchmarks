using System;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

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
        if (model == DatasetModel.Documents) return new MarketplaceGenerator();
        if (model == DatasetModel.Graph) return new SocialGenerator();
        throw new ArgumentOutOfRangeException(nameof(model));
    }
}
