using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

internal sealed class MarketplaceValues
{
    internal readonly ulong Seed;
    internal readonly long Products,Orders,Customers,Reviews;
    private readonly ZipfDistribution productZipf,customerZipf;
    private readonly IdPermutation productIds,customerIds;
    internal static readonly string[] Categories = ["electronics","apparel","books","grocery"];
    private static readonly string[][] Nouns = [
        ["camera","laptop","tablet","speaker","headset","monitor","keyboard","router","sensor"],
        ["shirt","jacket","sweater","scarf","coat","hat","sock","dress","trouser"],
        ["gardening","history","cooking","travel","mathematics","architecture","astronomy","photography","biology"],
        ["coffee","tea","rice","almond","walnut","oat","bean","honey","pepper"]];

    internal MarketplaceValues(ulong seed,double scale)
    {
        Seed=seed; Products=GenerationSession.Count(scale,100000); Orders=GenerationSession.Count(scale,250000);
        Customers=GenerationSession.Count(scale,150000); Reviews=GenerationSession.Count(scale,150000);
        productZipf=new(Products); customerZipf=new(Customers);
        productIds=new(Products,seed,CounterRandom.Stream("marketplace.product.popularity"));
        customerIds=new(Customers,seed,CounterRandom.Stream("marketplace.customer.popularity"));
    }
    internal EntityRandom Random(string stream,long id) => new(Seed,"marketplace."+stream,id);
    internal static string Key(string collection,long id) => collection+"/"+id.ToString(CultureInfo.InvariantCulture);
    internal long PopularProduct(ref EntityRandom r) => productIds.Permute(productZipf.Sample(ref r));
    internal long PopularCustomer(ref EntityRandom r) => customerIds.Permute(customerZipf.Sample(ref r));
    internal (string First,string Last,string Email,City City) Customer(long id)
    {
        var r=Random("customer.identity",id);
        string first=DatasetText.Pick(DatasetText.FirstNames,ref r),last=DatasetText.Pick(DatasetText.LastNames,ref r);
        return(first,last,first.ToLowerInvariant()+"."+last.ToLowerInvariant()+"."+id.ToString(CultureInfo.InvariantCulture)+"@example.test",
            DatasetText.Cities[r.Int(0,DatasetText.Cities.Length-1)]);
    }
    internal (string Category,string Name,int Variants,decimal BasePrice) Product(long id)
    {
        var r=Random("product.identity",id); int category=r.Int(0,3);
        return(Categories[category],DatasetText.Pick(DatasetText.Adjectives,ref r)+" "+DatasetText.Pick(Nouns[category],ref r),r.Int(1,4),r.Decimal(3,400));
    }
    internal (string Sku,decimal Price,int Stock) Variant(long id,int variant)
    {
        var p=Product(id); if(variant<0 || variant>=p.Variants) throw new ArgumentOutOfRangeException(nameof(variant));
        var r=new EntityRandom(Seed,CounterRandom.Stream("marketplace.variant")^(ulong)variant,id);
        return("MP-"+id.ToString(CultureInfo.InvariantCulture)+"-"+variant.ToString(CultureInfo.InvariantCulture),
            CommerceValues.Money(p.BasePrice*(1+variant*0.08m)),r.Int(0,500));
    }
    internal DateTime OrderDate(long id) { var r=Random("order.date",id); return DatasetTime.Sample(ref r,20); }
    internal (long Customer,DateTime Date,string Status,int Lines) Order(long id)
    {
        var r=Random("order.identity",id); long customer=PopularCustomer(ref r); DateTime date=OrderDate(id);
        return(customer,date,CommerceValues.OrderStatus(date,ref r),r.Int(2,5));
    }
    internal Dictionary<string,long> MaximumIds() => new(StringComparer.Ordinal)
        { ["customers"]=Customers,["products"]=Products,["orders"]=Orders,["reviews"]=Reviews };
}
