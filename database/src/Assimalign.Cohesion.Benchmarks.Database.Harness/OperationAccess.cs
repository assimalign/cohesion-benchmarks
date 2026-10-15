namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Whether a logical operation can mutate stored data; used to schedule pristine reads first.</summary>
public enum OperationAccess
{
    Read,
    Write
}
