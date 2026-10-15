using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Documents;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Documents;

/// <summary>Exercises real adapter writes and proves that read workloads retain their generated baseline.</summary>
public sealed class DocumentMutationIsolationTests
{
    private static int counter;
    [Fact]
    public async Task Mutations_SeveralClients_LeaveEveryBaselineReadDigestUnchanged()
    {
        string scratch = Path.GetFullPath(Environment.GetEnvironmentVariable("DBBENCH_DATA_SCRATCH") ?? Path.Combine(AppContext.BaseDirectory, "scratch"));
        string root = Path.GetFullPath(Path.Combine(scratch, "documents-" + Environment.ProcessId + "-" + Interlocked.Increment(ref counter)));
        Directory.CreateDirectory(root);
        try
        {
            var options = new DatasetGenerationOptions { Model = DatasetModel.Documents, Scale = 0.0001, OutputDirectory = Path.Combine(root, "data"), Threads = 1 };
            var manifest = await DatasetGenerators.Get(DatasetModel.Documents).GenerateAsync(options, null, CancellationToken.None);
            var model = ModelRegistration.Create();
            await using var target = model.CreateTarget("cohesion-documents-embedded", new TargetOptions
            { StorageDirectory = Path.Combine(root, "engine"), BufferPoolBytes = 32L * 1024 * 1024, BatchSize = 16 });
            await target.PrepareAsync(manifest, CancellationToken.None);
            await target.LoadAsync(manifest, DatasetFiles.DirectoryPath(options), null, CancellationToken.None);
            await target.FinalizeLoadAsync(CancellationToken.None);
            var before = await BaselineAsync();
            for (int client = 0; client < 3; client++)
            {
                await using var connection = await target.OpenClientAsync(client, CancellationToken.None);
                foreach (var operation in model.Operations)
                {
                    if (operation.Category != "write") continue;
                    foreach (bool warmup in new[] { false, true })
                    {
                        var stream = model.CreateParameterStream(manifest, manifest.Seed, operation.Id, client, new("verify", 1, 3, warmup));
                        for (int i = 0; i < 5; i++)
                        {
                            using var digest = new VerificationDigest(true);
                            await connection.ExecuteAsync(operation, stream.Next(), digest, CancellationToken.None);
                            digest.Complete().RowCount.ShouldBeGreaterThan(0);
                        }
                    }
                }
            }
            (await BaselineAsync()).ShouldBe(before);
            async Task<List<RowsDigest>> BaselineAsync()
            {
                var digests = new List<RowsDigest>();
                await using var connection = await target.OpenClientAsync(0, CancellationToken.None);
                foreach (var operation in model.Operations)
                {
                    if (operation.Category == "write" || target.Support[operation.Id].Level == SupportLevel.Unsupported) continue;
                    var stream = model.CreateParameterStream(manifest, manifest.Seed, operation.Id, 0, new("verify", 1, 1, false));
                    for (int i = 0; i < 5; i++)
                    {
                        using var digest = new VerificationDigest(operation.Ordered);
                        await connection.ExecuteAsync(operation, stream.Next(), digest, CancellationToken.None);
                        digests.Add(digest.Complete());
                    }
                }
                return digests;
            }
        }
        finally
        {
            // Only the freshly created, resolved child of the configured test scratch root is removed.
            if (!root.StartsWith(scratch.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Test cleanup escaped scratch root.");
            Directory.Delete(root, recursive: true);
        }
    }
}
