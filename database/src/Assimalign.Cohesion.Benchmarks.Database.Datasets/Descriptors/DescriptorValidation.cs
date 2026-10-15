using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

internal static class DescriptorValidation
{
    internal static void Scale(double scale)
    {
        if (!double.IsFinite(scale) || scale <= 0 || scale > 10000) throw new ArgumentOutOfRangeException(nameof(scale));
    }

    internal static void Manifest(DatasetManifest manifest, DatasetModel model)
    {
        if (manifest.Model != model) throw new ArgumentException("Manifest model does not match descriptor.", nameof(manifest));
        if (manifest.GeneratorVersion != GenerationSession.Version) throw new NotSupportedException("Manifest generator version cannot be recomputed by this implementation.");
    }

    internal static void Id(long id, long count)
    {
        if (id < 1 || id > count) throw new ArgumentOutOfRangeException(nameof(id));
    }
}
