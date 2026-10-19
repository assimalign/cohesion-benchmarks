# Engine settings

[catalog.json](catalog.json) is the executable source: image, budget, ports, volumes, environment, arguments and
readiness probe per engine, in the shared harness's `targets/catalog.json` schema (`CONVENTIONS.md`,
"Out-of-process targets"). The harness starts and stops them, allocates a free published port, waits for
readiness, and skips an engine this machine cannot start rather than failing the run. The per-engine
`settings.json` files keep each policy reviewable next to its reason in the table below.

Credentials are public, fixed, local-development values for a container on a loopback port. They are not secrets
and must never be reused anywhere else. The SQL Edge and RavenDB EULAs are accepted under the owner's decision of
2026-10-15.

Every container gets `--cpus 3 --memory 6g --memory-swap 6g`, declared per target and applied by the harness.
Swap is pinned to memory on purpose: an engine that exceeds its budget must fail rather than swap and post a
slower number. Data lives in a named volume per engine, never in the container layer, and a volume belongs to one
Cohesion build because the storage format refuses older files.

The two Cohesion targets are absent on purpose — the server host and the per-model adapters are not built yet
(see [../README.md](../README.md), "What is not here yet"). Until they exist these five engines have nothing to
be compared against, so starting them measures nothing.

| Engine | Non-default/shared setting | Reason |
|---|---|---|
| PostgreSQL 17 | shared_buffers=1536MB; effective_cache_size=4GB | Cache budget for 6 GiB |
| PostgreSQL 17 | work_mem=64MB; maintenance_work_mem=1GB | Bound query and index-build memory |
| PostgreSQL 17 | max_wal_size=8GB; checkpoint_timeout=15min | Reduce checkpoint frequency during the fixed run |
| PostgreSQL 17 | random_page_cost=1.1; max_parallel_workers_per_gather=2 | SSD cost assumption; shared 3-CPU budget |
| PostgreSQL 17 | synchronous_commit=on; fsync=on | Explicit durable acknowledgement (defaults retained) |
| SQL Edge | ACCEPT_EULA=Y; MSSQL_PID=Developer | Authorized local Developer edition |
| SQL Edge | MSSQL_MEMORY_LIMIT_MB=4608 | Leave 1.5 GiB for OS/overhead |
| SQL Edge | RECOVERY SIMPLE; DELAYED_DURABILITY DISABLED | No log backups; full commit durability; adapter applies prepare.sql |
| MongoDB 8 | wiredTigerCacheSizeGB=2.5; w:1,j:true in adapter | Shared cache budget and journal acknowledgement |
| RavenDB 7.1 | Setup.Mode=None; License.Eula.Accepted=true | No wizard; owner accepted EULA |
| RavenDB 7.1 | Security.UnsecuredAccessAllowed=PublicNetwork; ServerUrl=http://0.0.0.0:8080 | Local container network access; published ports bind loopback |
| RavenDB 7.1 | Wait for non-stale indexes in adapter | Include index build/catch-up in load finalization |
| Neo4j 5 Community | heap initial=max=1536m; pagecache=3g | Fixed heap and cache under the 6 GiB limit |
| Cohesion SQL/Graph/Documents | BufferPoolCapacity=3 GiB; Durability=Synchronous | Shared cache and durable commit policy; other engine options remain defaults |

Neo4j's environment names follow its [Docker configuration mapping](https://neo4j.com/docs/operations-manual/current/docker/configuration/).
RavenDB's [Setup.Mode documentation](https://docs.ravendb.net/7.1/server/configuration/core-configuration)
defines None as disabling startup setup.

## Readiness

`pg_isready`; the SQL Edge readiness log line; an authenticated `mongosh` ping; RavenDB `/build/version`; an
authenticated Neo4j `RETURN 1`. These are startup tests. They say the engine accepts connections, nothing about
whether it holds the right data — that is verification's job (DESIGN.md §7).

SQL Edge runs `sqlservr` as PID 1 rather than the image's default shell entry point, which ignores the stop
signal and gets the container killed after the timeout (exit 137). An unclean stop must never fall between a load
and a measured run.

## Smaller budgets

The table above is the comparison's budget and the only one its numbers may be quoted from. A smaller budget is
useful for a readiness check or a protocol smoke test, but it changes cache sizes and therefore the numbers, so a
run under one is not comparable with anything here and the harness records the observed budget with every result.

## The Cohesion side

Not built yet. When it is, it needs a server host and the per-model adapters written against the Cohesion API on
`main` at that time, composing each engine's own options — data root, buffer pool, commit durability — and its TCP
server, with the area's allow-all authenticator for local development. The paused run's mapping of those options
is in the `feature/database-benchmarks` branch at commit `ab502c7`; the option names moved with the concrete-types
rewrite, so re-derive it rather than copying it
([../docs/COHESION-CAPABILITIES.md](../docs/COHESION-CAPABILITIES.md#the-api-migration-1168-will-force)).