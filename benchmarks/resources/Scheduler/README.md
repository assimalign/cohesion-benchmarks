# Scheduler benchmarks

Measures `Scheduler.Cron` parsing, next occurrence, minute-by-minute evaluation and complete
occurrence enumeration over fixed UTC windows. Cronos is the baseline for identical five-field
expressions; setup verifies the occurrence sequences and evaluation counts. Existing history names
are preserved.

Standalone cases cover `Scheduler.Hosting` job registration and duplicate identity lookup, both
Cron and `Scheduler.Timer` provider lookup/enablement, and the real scheduling loops through
delegate dispatch. A deterministic advancing clock replaces elapsed waits; dispatch includes timer
continuation and cancellation/drain overhead. The suite opens no endpoints.

38 cases total (30 added). Not covered: host lifecycle, control-plane HTTP/authentication, retries,
parallel contention, real timer latency, time-zone/DST cron semantics, and application-model planning.
Cronos does not provide equivalent job registry or trigger execution semantics, so those run alone.

Run: `pwsh ./scripts/Invoke-Benchmarks.ps1 -Area Scheduler -Profile dry`
