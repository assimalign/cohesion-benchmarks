# Azure SQL Edge clean shutdown

## Diagnosis at R2, 2026-10-15

The image's default command leaves a shell as PID 1. The SQL process does not
receive the normal container stop signal, so Podman reaches its 40-second deadline
and kills the container. This was signal handling, not a SQL shutdown taking more
than 40 seconds, and not an out-of-memory kill.

Inspected image: `mcr.microsoft.com/azure-sql-edge:latest`, ARM64, image id
`9d0e27694fc9e99e3bfbe807eff0dc6a930d9d2af9d878845de7b819257c7ada`, digest
`sha256:3544cf1c6847c5cb60e071638c48b1b45d05a64eb2d88d0f83d44b397f40b186`.

Its entrypoint is `/opt/mssql/bin/permissions_check.sh`, whose last lines are:

```sh
# Execute the cmd from the dockerfile
exec "$@"
```

The image's default Cmd is:

```text
/bin/sh -c /opt/mssql/bin/launchpadd -usens=false -enableOutboundAccess=true -usesameuser=true -sqlGroup root -- -reparentOrphanedDescendants=true -useDefaultLaunchers=false & /app/asdepackage/AsdePackage & /opt/mssql/bin/sqlservr
```

Observed process tree before the fix:

```text
PID  PPID  COMMAND
1    0     /bin/sh -c ... & ... & /opt/mssql/bin/sqlservr
3    1     /opt/mssql/bin/launchpadd ...
5    1     /opt/mssql/bin/sqlservr
20   5     /opt/mssql/bin/sqlservr
```

`podman stop --time 40 dbbench-sqlserver-r2` took 40.8975033 seconds, returned
exit code 137, and inspection recorded `OOMKilled=false`. There was no SQL
termination/trace-stopped message. This reproduces SMOKE-R1A's failure. The
reproduction container and its empty data volume were removed before benchmark
loading; no unclean stop occurred between a load and any measured run.

## Catalog fix

The SQL Server target now overrides only Cmd, through its existing catalog
`arguments` field:

```json
"arguments": ["/opt/mssql/bin/sqlservr"]
```

The permissions entrypoint is preserved and its `exec` now makes `sqlservr` PID 1.
The stop signal reaches SQL Server, which drains and closes its databases. The
default launchpad/ASDE side services are not started: this suite benchmarks the
relational SQL engine and uses no external language, prediction or streaming
features from those services. Storage, durable commits, memory/CPU limits,
credentials and the normal 40-second stop deadline are unchanged.

Observed process tree with the catalog fix:

```text
PID  PPID  COMMAND
1    0     /opt/mssql/bin/sqlservr
4    1     /opt/mssql/bin/sqlservr
```

| Stop test | Seconds | Exit | OOMKilled |
|---|---:|---:|---|
| Original image command, before benchmark load | 40.8975033 | 137 | false |
| Direct sqlservr, before benchmark load | 1.1915592 | 0 | false |
| Direct sqlservr, after load, verification, latency and throughput | 2.5073202 | 0 | false |

Both fixed stops logged:

```text
SQL Server is terminating in response to a 'stop' request from Service Control Manager.
SQL Trace was stopped due to server shutdown. Trace ID = '1'.
```

The fixed container loaded SF 0.01, passed all 2,400 verification cases against
PostgreSQL and both Cohesion targets, and ran all latency operations plus 1/8-client
throughput without errors or timeouts. The final container and named data volume
were removed. No generic stop-script timeout increase or SIGKILL workaround was
needed.

## Reproduction commands and evidence

Run from the benchmark worktree; preserve task ownership and use a unique suffix.
Do not restore the broken command on a container containing benchmark data.

```powershell
podman image inspect mcr.microsoft.com/azure-sql-edge:latest
./database/scripts/Start-BenchEngine.ps1 -Target sqlserver -Memory 2g -Suffix -r2
podman exec dbbench-sqlserver-r2 ps -eo pid,ppid,args
podman exec dbbench-sqlserver-r2 cat /opt/mssql/bin/permissions_check.sh
$timer = [Diagnostics.Stopwatch]::StartNew()
podman stop --time 40 dbbench-sqlserver-r2
$timer.Stop()
$timer.Elapsed.TotalSeconds
podman inspect dbbench-sqlserver-r2 --format '{{.State.ExitCode}} {{.State.OOMKilled}}'
podman logs --tail 20 dbbench-sqlserver-r2
./database/scripts/Stop-BenchEngine.ps1 -Target sqlserver -Suffix -r2 -RemoveData
```

R2 logs are retained under ignored `database/artifacts/`:
`sql-edge-entrypoint.log`, `sql-edge-processes-original.log`,
`sql-edge-stop-original.log`, `sql-edge-processes-fixed.log`,
`sql-edge-stop-fixed.log`, and `sql-sqlserver-stop-final.log`.
See [SQL.md](../../docs/SQL.md) for the full workload and smoke record.
