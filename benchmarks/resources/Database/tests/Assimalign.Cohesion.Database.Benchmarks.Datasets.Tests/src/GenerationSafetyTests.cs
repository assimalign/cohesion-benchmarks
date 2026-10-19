using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Benchmarks.Datasets;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests;

/// <summary>Failure, overwrite and range boundary behavior.</summary>
public sealed class GenerationSafetyTests
{
    [Theory]
    [InlineData(DatasetModel.Sql)]
    [InlineData(DatasetModel.Documents)]
    [InlineData(DatasetModel.Graph)]
    public async Task Generate_ExplicitOverwrite_ReproducesTheDataset(DatasetModel model)
    {
        using var run=await DatasetRun.Generate(model,0.000001);
        string original=run.Manifest.Fingerprint;
        var options=new DatasetGenerationOptions { Model=model,Scale=run.Options.Scale,Seed=run.Options.Seed,Threads=3,OutputDirectory=run.Options.OutputDirectory,Overwrite=true };
        var manifest=await DatasetGenerators.Get(model).GenerateAsync(options,null,CancellationToken.None);
        manifest.Fingerprint.ShouldBe(original);
    }

    [Fact]
    public async Task Generate_PreCancelled_DoesNotReplaceAnExistingDataset()
    {
        using var run=await DatasetRun.Generate(DatasetModel.Sql,0.000001);
        var options=new DatasetGenerationOptions { Model=DatasetModel.Sql,Scale=run.Options.Scale,Threads=1,OutputDirectory=run.Options.OutputDirectory,Overwrite=true };
        await Should.ThrowAsync<OperationCanceledException>(async()=>await DatasetGenerators.Get(DatasetModel.Sql).GenerateAsync(options,null,new CancellationToken(true)));
        DatasetManifestFile.Load(Path.Combine(run.Directory,"manifest.json")).Fingerprint.ShouldBe(run.Manifest.Fingerprint);
    }

    [Fact]
    public void EnumerateParts_PathTraversal_IsRejected()
    {
        var manifest=new DatasetManifest { GeneratorVersion=1,Model=DatasetModel.Sql,Name="commerce",Seed=1,Scale=1,
            Tables=new[] { new DatasetTableManifest { Name="orders",Rows=1,Bytes=1,
                Parts=new[] { new DatasetPartManifest { Path="../escape.csv",Rows=1,Bytes=1,Sha256=new string('0',64) } } } } };
        using var iterator=DatasetFiles.EnumerateParts(@"C:\cw\bench\data-scratch",manifest,"orders").GetEnumerator();
        Should.Throw<InvalidDataException>(()=>iterator.MoveNext());
    }

    [Fact]
    public void Text_ExactLengthBounds_RespectsTheRequestedLength()
    {
        var random=new EntityRandom(4,"exact-text",1);
        for(int i=1;i<=50;i++) DatasetText.Paragraph(ref random,i,i,i).Length.ShouldBe(i);
    }
}
