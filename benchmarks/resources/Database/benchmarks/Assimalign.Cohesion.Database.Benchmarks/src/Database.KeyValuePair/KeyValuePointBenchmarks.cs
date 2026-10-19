using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.Database.KeyValuePair;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Database.Benchmarks.Database.KeyValuePair;

/// <summary>
/// Point reads against the embedded, in-memory key-value engine through one session: what a resource that embeds the
/// engine (a configuration store, a scheduler's state) pays per lookup, from the typed request through the MVCC
/// snapshot read to the decoded entry. 1,024 keys with 128-byte values; reads walk the keys in turn.
/// </summary>
/// <remarks>
/// There is no write benchmark yet. Each autocommit put journals a full 8 KB after-image of every page it touches
/// (about 47 KB per 128-byte put), and the in-memory journal is a single memory stream: under continuous writes it
/// passes the 2 GB stream limit after roughly 45,000 puts and every later put fails with "Stream was too long", with
/// the checkpoint interval at 30 seconds or at one. Add a <c>Put</c> benchmark once the journal stays bounded under
/// sustained writes.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("KeyValue")]
public class KeyValuePointBenchmarks
{
    private const int KeyCount = 1024;

    private KeyValueDatabaseEngine _engine = null!;
    private KeyValueDatabase _database = null!;
    private KeyValueDatabaseSession _session = null!;
    private byte[][] _keys = null!;
    private int _next;

    [GlobalSetup]
    public void Setup()
    {
        _engine = KeyValueDatabaseEngine.Create("benchmarks", new KeyValueDatabaseEngineOptions());
        _database = _engine.CreateDatabaseAsync("benchmarks").AsTask().GetAwaiter().GetResult();
        _session = _database.CreateSessionAsync().AsTask().GetAwaiter().GetResult();

        byte[] value = new byte[128];
        _keys = new byte[KeyCount][];
        for (int index = 0; index < KeyCount; index++)
        {
            _keys[index] = Encoding.UTF8.GetBytes($"settings:feature:{index:D4}");
            _database.PutAsync(_session, _keys[index], value).AsTask().GetAwaiter().GetResult();
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _session.DisposeAsync().AsTask().GetAwaiter().GetResult();
        _engine.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    [Benchmark]
    public ValueTask<KeyValueEntry?> Get() => _database.GetAsync(_session, _keys[_next++ & (KeyCount - 1)]);
}
