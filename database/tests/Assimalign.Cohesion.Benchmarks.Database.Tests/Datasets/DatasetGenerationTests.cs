using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets;

/// <summary>Whole-dataset determinism, calibration, manifests and opt-in scale-one measurements.</summary>
public sealed class DatasetGenerationTests(ITestOutputHelper output)
{
    [Theory]
    [InlineData(DatasetModel.Sql)]
    [InlineData(DatasetModel.Documents)]
    [InlineData(DatasetModel.Graph)]
    public async Task Generate_ThreadCountsAndSeeds_HasDeterministicContent(DatasetModel model)
    {
        using var first=await DatasetRun.Generate(model,0.01,1);
        using var parallel=await DatasetRun.Generate(model,0.01,4);
        using var changed=await DatasetRun.Generate(model,0.01,2,1234567);
        File.ReadAllBytes(Path.Combine(first.Directory,"manifest.json")).ShouldBe(File.ReadAllBytes(Path.Combine(parallel.Directory,"manifest.json")));
        first.Manifest.Fingerprint.ShouldNotBe(changed.Manifest.Fingerprint);
        foreach(var table in first.Manifest.Tables)
        {
            long rows=0,bytes=0;
            foreach(var part in table.Parts)
            {
                string path=Path.Combine(first.Directory,part.Path);
                using var stream=File.OpenRead(path);
                Convert.ToHexStringLower(SHA256.HashData(stream)).ShouldBe(part.Sha256);
                new FileInfo(path).Length.ShouldBe(part.Bytes);
                rows+=part.Rows; bytes+=part.Bytes;
                File.ReadAllBytes(path).ShouldBe(File.ReadAllBytes(Path.Combine(parallel.Directory,part.Path)));
            }
            rows.ShouldBe(table.Rows); bytes.ShouldBe(table.Bytes);
        }
        DatasetManifest loaded=DatasetManifestFile.Load(Path.Combine(first.Directory,"manifest.json"));
        loaded.Fingerprint.ShouldBe(first.Manifest.Fingerprint);
        await Should.ThrowAsync<IOException>(async()=>await DatasetGenerators.Get(model).GenerateAsync(first.Options,null,CancellationToken.None));
    }

    [Theory]
    [InlineData(DatasetModel.Sql,0.41)]
    [InlineData(DatasetModel.Documents,0.41)]
    [InlineData(DatasetModel.Graph,1.34)]
    public async Task Generate_MultiplePartRanges_IsIndependentOfScheduling(DatasetModel model,double scale)
    {
        using var first=await DatasetRun.Generate(model,scale,1);
        using var parallel=await DatasetRun.Generate(model,scale,4);
        bool multiple=false;
        foreach(var table in first.Manifest.Tables) if(table.Parts.Count>1) multiple=true;
        multiple.ShouldBeTrue();
        File.ReadAllBytes(Path.Combine(first.Directory,"manifest.json")).ShouldBe(File.ReadAllBytes(Path.Combine(parallel.Directory,"manifest.json")));
        output.WriteLine($"{model} multi-part determinism: {first.Bytes:N0} bytes; 1 worker {first.Seconds:F3}s, 4 workers {parallel.Seconds:F3}s.");
    }

    [Theory]
    [InlineData(DatasetModel.Sql)]
    [InlineData(DatasetModel.Documents)]
    [InlineData(DatasetModel.Graph)]
    public async Task Generate_ScalePointZeroFive_ExtrapolatesToOneDecimalGigabyte(DatasetModel model)
    {
        using var run=await DatasetRun.Generate(model,0.05,2);
        double extrapolated=run.Bytes/0.05;
        output.WriteLine($"{model}: SF0.05={run.Bytes:N0} bytes; SF1 estimate={extrapolated:N0}; elapsed={run.Seconds:F3}s.");
        extrapolated.ShouldBeInRange(850000000,1150000000);
    }

    [LongDatasetFact]
    public async Task Generate_ScaleOne_ReportsActualSizesAndCounts()
    {
        foreach(DatasetModel model in new[]{DatasetModel.Sql,DatasetModel.Documents,DatasetModel.Graph})
        {
            using var run=await DatasetRun.Generate(model,1,4);
            output.WriteLine($"MEASURE {model} bytes={run.Bytes} seconds={run.Seconds:F3} fingerprint={run.Manifest.Fingerprint}");
            foreach(var table in run.Manifest.Tables) output.WriteLine($"TABLE {model}/{table.Name} rows={table.Rows} bytes={table.Bytes} maxId={table.MaximumId} parts={table.Parts.Count}");
            run.Bytes.ShouldBeInRange(850000000L,1150000000L);
            if(model==DatasetModel.Graph)
                foreach(var table in run.Manifest.Tables)
                    if(table.Name=="person_knows_person") (2.0*table.Rows/30000).ShouldBeInRange(35,45);
            DatasetManifestFile.Load(Path.Combine(run.Directory,"manifest.json")).Fingerprint.ShouldBe(run.Manifest.Fingerprint);
        }
    }

    [Fact]
    public void Descriptor_IncompatibleVersion_RejectsRecomputation()
    {
        var manifest=new DatasetManifest { GeneratorVersion=999,Model=DatasetModel.Sql,Name="commerce",Scale=1,Seed=1,Tables=Array.Empty<DatasetTableManifest>() };
        Should.Throw<NotSupportedException>(()=>new CommerceDescriptor(manifest));
        Should.Throw<ArgumentOutOfRangeException>(()=>new CommerceDescriptor(1,double.NaN));
    }
}
