# FileSystem benchmarks

44 cases: the six existing cases plus 38 additions. Existing names and inputs, including
`PhysicalFileSystemBenchmarks.ReadFile`, are unchanged.

| Package | Added cases | Work measured |
| --- | ---: | --- |
| FileSystem | 2 | Directory-relative path merge and resolution, shallow/deep |
| FileSystem.Aggregate | 6 | Lookup, unmounted miss, enumeration with 4/16 mounted providers |
| FileSystem.Globbing | 8 | Reused matcher versus build-and-first-match, two pattern shapes |
| FileSystem.InMemory | 6 | Read, overwrite, create/write/delete at 4 KiB/1 MiB |
| FileSystem.Physical | 16 | Read, cached-file-info read, overwrite at 4 KiB/1 MiB; lookup and recursive enumeration |

Counterparts reuse the existing Microsoft.Extensions.FileSystemGlobbing and System.IO dependencies.
Glob pairs match the same candidate and return the same Boolean. Physical pairs use the same bytes,
stream modes and sharing; enumeration creates file-system info objects on both sides. Physical paths
include a deeper tree alongside the original shallow read. The cached-file-info read and lookup-only
cases help separate file lookup/path overhead from opening and copying. These are OS-cache workloads,
and writes close the stream without requesting durable storage.

Aggregate is a mount router, not a search-every-source overlay. Its in-memory providers are owned and
disposed by the aggregate. The pinned in-memory recursive enumerator emits files without intermediate
directories; the setup check explicitly accounts for that. No invented counterparts are used for
in-memory tree semantics, mounted routing, or provider-relative directory resolution.

Not covered: IsolatedStorage, watches, asynchronous I/O, cross-provider copy/move, in-memory locking,
and cold disk/durable-write performance. IsolatedStorage's public constructors always open the
OS-managed store and provide no temporary-root override; measuring it would violate this task's
restriction to files under a temporary directory created by the benchmark.

Validation:

```powershell
dotnet build benchmarks/libraries/FileSystem/benchmarks/Assimalign.Cohesion.FileSystem.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area FileSystem -Profile dry
```
