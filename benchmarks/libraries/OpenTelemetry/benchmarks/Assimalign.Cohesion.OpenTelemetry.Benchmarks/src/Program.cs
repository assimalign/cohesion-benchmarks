using Assimalign.Cohesion.Benchmarks;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks;

public static class Program
{
    public static int Main(string[] args) => BenchmarkProgram.Run(args, typeof(Program).Assembly);
}
