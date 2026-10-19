namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Inclusive generated id bounds. Composite children use their owner's id; a zero maximum means no ids.</summary>
public readonly record struct EntityIdRange(long Minimum, long Maximum)
{
    public bool Contains(long id) => Maximum > 0 && id >= Minimum && id <= Maximum;
}
