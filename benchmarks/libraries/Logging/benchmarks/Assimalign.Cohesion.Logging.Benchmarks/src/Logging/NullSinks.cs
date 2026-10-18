using System;

using ExtensionsLogging = Microsoft.Extensions.Logging;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging;

/// <summary>
/// Sinks that accept every entry and do nothing with it, one per logging stack, so a benchmark measures the
/// pipeline in front of the sink (level gate, entry construction, fan-out, scopes) and not a transport. Each opens a
/// scope as a small object, as a real sink's scope does.
/// </summary>
internal static class NullSinks
{
    public sealed class CohesionProvider : LoggerProvider
    {
        public override string Name => "null";

        protected override Logger CreateCore(string category) => new CohesionLogger(category);
    }

    public sealed class ExtensionsProvider : ExtensionsLogging.ILoggerProvider
    {
        public ExtensionsLogging.ILogger CreateLogger(string categoryName) => new ExtensionsLogger();

        public void Dispose()
        {
        }
    }

    private sealed class CohesionLogger(string category) : Logger(category)
    {
        protected override void WriteCore(ILoggerEntry entry)
        {
        }

        protected override IScopedLogger BeginScopeCore(ILoggerEntry entry) => new CohesionScopedLogger(Category, entry.Id);
    }

    private sealed class CohesionScopedLogger(string category, LogId parentId) : ScopedLogger(category, parentId)
    {
        protected override void WriteCore(ILoggerEntry entry)
        {
        }

        protected override IScopedLogger BeginScopeCore(ILoggerEntry entry) => new CohesionScopedLogger(Category, entry.Id);
    }

    private sealed class ExtensionsLogger : ExtensionsLogging.ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => new ExtensionsScope();

        public bool IsEnabled(ExtensionsLogging.LogLevel logLevel) => logLevel != ExtensionsLogging.LogLevel.None;

        public void Log<TState>(
            ExtensionsLogging.LogLevel logLevel,
            ExtensionsLogging.EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }
    }

    private sealed class ExtensionsScope : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
