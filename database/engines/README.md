# Engine settings

catalog.json is the executable source of engine identities, development credentials, network aliases, ports,
volumes, arguments, readiness probes and settings. Per-engine settings.json files make each policy reviewable.
Credentials are public, fixed, local-development values only. SQL Edge and RavenDB EULAs are accepted under the
owner's decision of 2026-10-15. No real deployment secrets belong here.

All containers get --cpus 3 --memory 6g --memory-swap 6g. Data lives in named volumes. :U initializes ownership
for images that run as a non-root user. The scripts check a worktree ownership label before removal and never
prune shared resources. -Recreate replaces only this worktree's labeled container. -RemoveData explicitly
removes its volume, including the shared embedded/TCP Cohesion data when selected. --image-volume ignore
prevents image-declared auxiliary paths from allocating anonymous volumes; database data uses the explicit named mount.

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

Readiness: pg_isready; SQL Edge readiness log plus a TCP connection; authenticated mongosh ping; RavenDB
/build/version; authenticated Neo4j RETURN 1; Cohesion's READY line
followed by an actual client handshake in the driver image. These are startup tests, not load/verification
substitutes.

## Development memory profile

When -Memory is below 6g, Start-BenchEngine labels the container development and reduces startup allocations:
PostgreSQL shared_buffers=25%, effective_cache_size=50%, maintenance_work_mem=max(64 MiB,12.5%), work_mem=16MB;
SQL Edge memory limit=75%; MongoDB cache=max(0.25 GiB,40%); Neo4j heap=18.75% and page cache=25%;
Cohesion buffer pool=50%. Durability stays unchanged. RavenDB keeps its defaults.
The small profile is only for readiness and tiny protocol smoke; official runs use the exact table above.

## Cohesion server option mapping

Paths below are relative to COHESION_ROOT at bf20be5eb.

| CLI option | Public composition/engine option |
|---|---|
| --model sql/graph | SqlDatabaseEngine.CreateBuilder / GraphDatabaseEngine.CreateBuilder |
| --engine-name | Builder name; SqlDatabaseEngineBuilder.cs:89, GraphDatabaseEngineBuilder.cs:71 |
| --database | AddDatabase; SQL builder:156, Graph builder:107; opens existing files or creates an empty database |
| --data | SqlDatabaseEngineOptions.RootPath:196; GraphDatabaseEngineOptions.RootPath:164 |
| --buffer-pool-mb | SQL BufferPoolCapacity:128; Graph BufferPoolCapacity:122; MiB converted to bytes |
| --durability | SQL Durability:32; Graph Durability:28; StorageCommitDurability enum |
| --port | SQL SqlDatabaseServerOptionsExtensions.Listen(Uri):30; Graph TcpConnectionListenerOptions.EndPoint:16 |

Engine option files are under resources/Database/Assimalign.Cohesion.Database.<model>/src/.
TcpConnectionListenerOptions is under libraries/Connections/Assimalign.Cohesion.Connections.Tcp/src/.
Authentication uses DatabaseAuthenticator.AllowAll (Database.Security/src/DatabaseAuthenticator.cs:41).
DatabaseServer.StartAsync:89 binds the listener before READY; StopAsync:139 drains it before engine disposal.
No Database.Hosting, Web.Hosting or Web.Health references are required by this standalone composition.

The host applies no graph schema. Embedded loaders create indexes through GraphSchema.CreateIndexAsync and
close the engine before TCP opens the same volume. SQL also reuses embedded-loaded files. Both engines expose
buffer-pool and durability controls; neither needs an exception to the shared setting policy.
