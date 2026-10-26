using System;

namespace Assimalign.Cohesion.Benchmarks;

/// <summary>
/// Keeps one benchmark of a class opted into NativeAOT runs (<see cref="BenchmarkRuntimes.Category"/>) out of them: a
/// counterpart that cannot run natively compiled, such as a library that compiles expression trees at run time. The
/// JIT run measures it as usual.
/// </summary>
/// <remarks>
/// A method-level category would filter the same way, but it also changes the case's logical group, and with it which
/// baseline its category compares against on the JIT. This attribute leaves the JIT run untouched. Setup that builds
/// the counterpart has to skip it too when <see cref="System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported"/>
/// is false.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class JitOnlyAttribute : Attribute
{
}
