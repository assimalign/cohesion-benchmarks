using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>The last lines a process wrote, for error messages and the <c>log</c> readiness probe.</summary>
internal sealed class OutputTail
{
    private readonly int _capacity;
    private readonly Queue<string> _lines = new();
    private readonly object _sync = new();
    private Regex? _pattern;
    private TaskCompletionSource? _matched;

    public OutputTail(int lines) => _capacity = lines;

    public void Add(string? line)
    {
        if (line is null)
        {
            return;
        }

        lock (_sync)
        {
            if (_lines.Count == _capacity)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(line);
            if (_pattern is not null && _pattern.IsMatch(line))
            {
                _matched?.TrySetResult();
            }
        }
    }

    /// <summary>Completes once a line matching <paramref name="pattern"/> is written (including one already seen).</summary>
    public Task WaitForAsync(string pattern)
    {
        lock (_sync)
        {
            _pattern = new Regex(pattern, RegexOptions.CultureInvariant);
            _matched = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            foreach (string line in _lines)
            {
                if (_pattern.IsMatch(line))
                {
                    _matched.TrySetResult();
                    break;
                }
            }

            return _matched.Task;
        }
    }

    public string Format()
    {
        lock (_sync)
        {
            return _lines.Count == 0 ? string.Empty : Environment.NewLine + "--- last output ---" + Environment.NewLine + string.Join(Environment.NewLine, _lines);
        }
    }
}
