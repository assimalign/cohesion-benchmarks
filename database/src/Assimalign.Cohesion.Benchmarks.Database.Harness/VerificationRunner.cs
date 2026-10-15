using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Runs fixed deterministic cases and compares every supported pair without treating unsupported as empty results.</summary>
public static class VerificationRunner
{
    public static async Task<VerificationFile> VerifyAsync(IBenchmarkModel model, IBenchmarkTarget target,
        DatasetManifest manifest, string manifestHash, IReadOnlyList<OperationDescriptor> operations,
        CancellationToken token = default)
    {
        var results = new List<VerificationOperation>();
        foreach (var operation in operations)
        {
            OperationSupport support = target.Support[operation.Id];
            support.Validate();
            var cases = new List<RowsDigest>();
            string? error = null;
            if (support.Level != SupportLevel.Unsupported)
            {
                try
                {
                    await using var client = await target.OpenClientAsync(0, token).ConfigureAwait(false);
                    var parameters = model.CreateParameterStream(manifest, manifest.Seed, operation.Id, 0, new("verify", 1, 1, false));
                    for (int i = 0; i < 100; i++)
                    {
                        using var digest = new VerificationDigest(operation.Ordered);
                        object parameter = parameters.Next();
                        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(token);
                        deadline.CancelAfter(TimeSpan.FromSeconds(operation.TimeoutClass == TimeoutClass.Point ? 30 : 600));
                        await client.ExecuteAsync(operation, parameter, digest, deadline.Token).ConfigureAwait(false);
                        deadline.Token.ThrowIfCancellationRequested();
                        cases.Add(digest.Complete());
                    }
                }
                catch (Exception ex) when (ex is not OutOfMemoryException && !token.IsCancellationRequested)
                { error = ex.GetType().Name + ": " + ex.Message; }
            }
            results.Add(new(operation.Id, support, cases, error));
        }
        return new() { Model = model.Id, Target = target.Descriptor.Id, DatasetManifestSha256 = manifestHash,
            Seed = manifest.Seed, Operations = results };
    }

    public static List<VerificationMismatch> Compare(IReadOnlyList<VerificationFile> files)
    {
        var mismatches = new List<VerificationMismatch>();
        foreach (var file in files)
        {
            if (file.SchemaVersion != 1) throw new ArgumentException("Unsupported verification schema.");
            foreach (var op in file.Operations)
                if (op.Support.Level != SupportLevel.Unsupported && (op.Error != null || op.Cases.Count != file.ParameterCount || file.ParameterCount != 100))
                    mismatches.Add(new(file.Model, op.Operation, file.Target, file.Target, op.Error ?? "Incomplete fixed parameter set."));
        }
        for (int a = 0; a < files.Count; a++)
        for (int b = a + 1; b < files.Count; b++)
        {
            var left = files[a]; var right = files[b];
            if (left.Model != right.Model) continue;
            if (left.DatasetManifestSha256 != right.DatasetManifestSha256 || left.Seed != right.Seed)
            {
                mismatches.Add(new(left.Model, "*", left.Target, right.Target, "Dataset hash or seed differs."));
                continue;
            }
            var ids = left.Operations.Select(o => o.Operation).Union(right.Operations.Select(o => o.Operation), StringComparer.Ordinal);
            foreach (string id in ids)
            {
                var x = left.Operations.SingleOrDefault(o => o.Operation == id);
                var y = right.Operations.SingleOrDefault(o => o.Operation == id);
                if (x == null || y == null)
                    mismatches.Add(new(left.Model, id, left.Target, right.Target, "Operation missing from a verification file."));
                else if (x.Support.Level != SupportLevel.Unsupported && y.Support.Level != SupportLevel.Unsupported)
                {
                    for (int i = 0; i < Math.Min(x.Cases.Count, y.Cases.Count); i++)
                        if (x.Cases[i] != y.Cases[i])
                            mismatches.Add(new(left.Model, id, left.Target, right.Target,
                                $"Parameter {i}: {x.Cases[i].Sha256} ({x.Cases[i].RowCount} rows) != {y.Cases[i].Sha256} ({y.Cases[i].RowCount} rows)."));
                }
            }
        }
        return mismatches;
    }
}
