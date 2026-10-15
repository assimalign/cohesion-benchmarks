using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets;

/// <summary>Distribution, bounds, permutation and entity independence checks.</summary>
public sealed class PrimitiveTests
{
    [Fact]
    public void Counter_EntitySequences_AreIndependentAndRepeatable()
    {
        var a=new EntityRandom(9,"customer",13); var b=new EntityRandom(9,"customer",13);
        var other=new EntityRandom(9,"customer",14);
        for(int i=0;i<1000;i++) { a.NextUInt64().ShouldBe(b.NextUInt64()); other.NextUInt64(); }
        CounterRandom.Value(9,3,1,0).ShouldNotBe(CounterRandom.Value(9,3,1,1));
        CounterRandom.Value(9,3,1).ShouldNotBe(CounterRandom.Value(10,3,1));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(17)]
    [InlineData(300)]
    [InlineData(1001)]
    [InlineData(65536)]
    public void Permute_CompleteDomain_IsBijectiveAndInvertible(int count)
    {
        var p=new IdPermutation(count,123,456); var seen=new HashSet<long>();
        for(long i=1;i<=count;i++) { long id=p.Permute(i); id.ShouldBeInRange(1,count); seen.Add(id).ShouldBeTrue(); p.Invert(id).ShouldBe(i); }
        if(count>100) p.Permute(1).ShouldNotBe(1);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(1.00000001)]
    [InlineData(0.99999999)]
    [InlineData(1.5)]
    [InlineData(8.0)]
    public void Zipf_KnownProbabilities_MatchesMassWithoutACdf(double exponent)
    {
        const int count=50,samples=100000;
        var distribution=new ZipfDistribution(count,exponent); var r=new EntityRandom(42,"zipf",1);
        var frequencies=new int[count]; double sum=0;
        for(int k=1;k<=count;k++) sum+=Math.Pow(k,-exponent);
        for(int i=0;i<samples;i++) frequencies[distribution.Sample(ref r)-1]++;
        for(int k=1;k<=count;k++)
        {
            double probability=Math.Pow(k,-exponent)/sum;
            Math.Abs(frequencies[k-1]-samples*probability).ShouldBeLessThan(7*Math.Sqrt(samples*probability*(1-probability))+8);
        }
        var large=new ZipfDistribution(1000000000000,exponent);
        for(int i=0;i<1000;i++) large.Sample(ref r).ShouldBeInRange(1L,1000000000000L);
    }

    [Fact]
    public void UniformAndNormal_LargeSample_RespectsBoundsAndMoments()
    {
        var r=new EntityRandom(44,"primitives",1); double sum=0,squares=0; var counts=new int[7];
        for(int i=0;i<100000;i++)
        {
            counts[r.Int(0,6)]++;
            r.Long(long.MinValue,long.MaxValue); r.Long(-5,8).ShouldBeInRange(-5,8);
            decimal money=r.Decimal(-10,20); money.ShouldBeInRange(-10,20); (money*100).ShouldBe(decimal.Truncate(money*100));
            double normal=r.Normal(3,2); sum+=normal; squares+=normal*normal;
        }
        (sum/100000).ShouldBeInRange(2.97,3.03);
        (squares/100000-Math.Pow(sum/100000,2)).ShouldBeInRange(3.9,4.1);
        foreach(int count in counts) count.ShouldBeInRange(13500,15000);
        var category=new WeightedCategorical(0,1,3); int one=0;
        for(int i=0;i<100000;i++) { int value=category.Sample(ref r); value.ShouldNotBe(0); if(value==1) one++; }
        one.ShouldBeInRange(24000,26000);
    }

    [Fact]
    public void Calendar_SeasonalityAndSequences_AreUtcBoundedAndMonotonic()
    {
        var r=new EntityRandom(5,"time",7); int peak=0,evening=0;
        for(int i=0;i<30000;i++)
        {
            DateTime date=DatasetTime.Sample(ref r,10);
            date.Kind.ShouldBe(DateTimeKind.Utc); date.ShouldBeInRange(DatasetTime.Start,DatasetTime.End);
            (date.Ticks%TimeSpan.TicksPerSecond).ShouldBe(0);
            if(date.Month>=11) peak++;
            if(date.Hour>=17 && date.Hour<=21) evening++;
            DateTime next=DatasetTime.After(date,ref r,86400,2);
            next.ShouldBeGreaterThan(date); next.ShouldBeLessThanOrEqualTo(DatasetTime.End.AddSeconds(-2));
        }
        peak.ShouldBeInRange(9500,11000); evening.ShouldBeGreaterThan(9000);
    }

    [Fact]
    public void Text_EmbeddedVocabularyAndLengths_HasLongTails()
    {
        DatasetText.Words.Length.ShouldBeGreaterThan(2500);
        var r=new EntityRandom(1,"text",1); int longTexts=0;
        for(int i=0;i<1000;i++)
        {
            string value=DatasetText.Paragraph(ref r,100,2000,400);
            value.Length.ShouldBeInRange(100,2000); if(value.Length>1000) longTexts++;
        }
        longTexts.ShouldBeGreaterThan(50);
    }
}
