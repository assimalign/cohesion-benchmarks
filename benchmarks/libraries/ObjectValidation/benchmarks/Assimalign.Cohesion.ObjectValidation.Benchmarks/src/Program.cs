using Assimalign.Cohesion.Benchmarks;

namespace Assimalign.Cohesion.ObjectValidation.Benchmarks;

public static class Program
{
    public static int Main(string[] args) => BenchmarkProgram.Run(args, typeof(Program).Assembly);
}
