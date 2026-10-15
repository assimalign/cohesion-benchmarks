namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>A deterministic stream keyed by seed, operation and client; Next always runs outside the timer.</summary>
public interface IParameterStream
{
    object Next();
}
