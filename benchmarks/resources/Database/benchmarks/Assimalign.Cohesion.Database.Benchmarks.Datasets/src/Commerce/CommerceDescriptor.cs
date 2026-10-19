using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Pure commerce parameter recomputation used directly by the generator. IDs start at one.</summary>
public sealed class CommerceDescriptor
{
    public ulong Seed { get; }
    public double Scale { get; }
    public readonly long Customers, Addresses, Products, Suppliers, Orders, Reviews;
    public IReadOnlyList<DatasetTableManifest>? GeneratedTables { get; }
    private readonly ZipfDistribution customerZipf, productZipf;
    private readonly IdPermutation customerIds, productIds;
    internal static readonly WeightedCategorical Ratings = new(2, 4, 12, 32, 50);
    internal static readonly string[] Channels = ["web", "mobile", "store", "partner"];

    public CommerceDescriptor(DatasetManifest manifest) : this(manifest.Seed, manifest.Scale)
    {
        DescriptorValidation.Manifest(manifest, DatasetModel.Sql);
        GeneratedTables = manifest.Tables;
    }

    public CommerceDescriptor(ulong seed, double scale)
    {
        DescriptorValidation.Scale(scale);
        Scale = scale;
        Seed = seed;
        Customers = GenerationSession.Count(scale,250000); Addresses = Math.Max(Customers, GenerationSession.Count(scale,375000));
        Products = GenerationSession.Count(scale,50000); Suppliers = GenerationSession.Count(scale,2000);
        Orders = GenerationSession.Count(scale,1000000); Reviews = GenerationSession.Count(scale,400000);
        customerZipf = new(Customers); productZipf = new(Products);
        customerIds = new(Customers,seed,CounterRandom.Stream("commerce.customer.popularity"));
        productIds = new(Products,seed,CounterRandom.Stream("commerce.product.popularity"));
    }

    internal EntityRandom Random(string stream, long id) => new(Seed, stream, id);
    internal long PopularCustomer(ref EntityRandom r) => customerIds.Permute(customerZipf.Sample(ref r));
    internal long PopularProduct(ref EntityRandom r) => productIds.Permute(productZipf.Sample(ref r));
    public (string First, string Last, string Email, City City) Customer(long id)
    {
        DescriptorValidation.Id(id, Customers);
        var r = Random("commerce.customer.identity",id);
        string first = DatasetText.Pick(DatasetText.FirstNames,ref r), last = DatasetText.Pick(DatasetText.LastNames,ref r);
        return (first,last, first.ToLowerInvariant() + "." + last.ToLowerInvariant() + "." + id.ToString(CultureInfo.InvariantCulture) + "@example.test",
            DatasetText.Cities[r.Int(0,DatasetText.Cities.Length-1)]);
    }

    public (long Category, long Supplier, string Brand, string Name, decimal Price) Product(long id)
    {
        DescriptorValidation.Id(id, Products);
        var r = Random("commerce.product.identity",id);
        long category = r.Int(71,400), supplier = r.Long(1,Suppliers);
        string brand = DatasetText.Pick(DatasetText.Brands,ref r);
        string name = DatasetText.Pick(DatasetText.Adjectives,ref r) + " " + DatasetText.Pick(DatasetText.ProductNouns,ref r);
        return (category,supplier,brand,name,r.Decimal(2,600));
    }

    public static long? CategoryParent(long id) { DescriptorValidation.Id(id,400); return id <= 10 ? null : id <= 70 ? 1 + (id - 11) % 10 : 11 + (id - 71) % 60; }
    public static string CategoryName(long id) { DescriptorValidation.Id(id,400); return DatasetText.Topics[(id - 1) % DatasetText.Topics.Length] + " " + id.ToString(CultureInfo.InvariantCulture); }
    public long AddressCustomer(long id) { DescriptorValidation.Id(id,Addresses); return id <= Customers ? id : id - Customers; }
    public DateTime OrderDate(long id) { DescriptorValidation.Id(id,Orders); var r = Random("commerce.order.date",id); return DatasetTime.Sample(ref r,20); }
    public string CustomerEmail(long id) => Customer(id).Email;
    public long ProductCategory(long id) => Product(id).Category;
    public string ProductBrand(long id) => Product(id).Brand;

    public (long Customer, long Address, DateTime Date, string Status, string Channel, int Lines) Order(long id)
    {
        var r = Random("commerce.order.identity",id);
        long customer = PopularCustomer(ref r);
        long address = customer <= Addresses-Customers && r.Chance(0.25) ? Customers+customer : customer;
        DateTime date = OrderDate(id);
        return (customer,address,date,OrderStatus(date,ref r),Channels[r.Int(0,3)],r.Int(1,6));
    }

    internal static string OrderStatus(DateTime date, ref EntityRandom r)
    {
        double roll = r.NextDouble(), age = (DatasetTime.End-date).TotalDays;
        if (roll < 0.03) return "cancelled";
        if (roll < 0.05 && age > 14) return "returned";
        if (age < 1) return r.Chance(0.5) ? "placed" : "paid";
        if (age < 4) return r.Chance(0.5) ? "paid" : "shipped";
        if (age < 10) return r.Chance(0.8) ? "shipped" : "delivered";
        return roll < 0.06 ? "paid" : "delivered";
    }

    public (long Product, int Quantity, decimal Price, decimal Discount, decimal Total) Line(long orderId, int line)
    {
        DescriptorValidation.Id(orderId,Orders);
        DescriptorValidation.Id(line,6);
        var r = new EntityRandom(Seed,CounterRandom.Stream("commerce.order.line") ^ (ulong)line,orderId);
        long product = PopularProduct(ref r);
        decimal price = Product(product).Price;
        int quantity = r.Int(1,4);
        decimal discount = r.Chance(0.15) ? Money(price*quantity*0.1m) : 0;
        return (product,quantity,price,discount,price*quantity-discount);
    }

    internal static decimal Money(decimal value) => decimal.Round(value,2,MidpointRounding.AwayFromZero);
    internal static decimal TaxRate(string country) => country switch { "US" => 0.07m, "CA" => 0.13m, "GB" => 0.20m, "DE" => 0.19m, "FR" => 0.20m, "AU" => 0.10m, "JP" => 0.10m, _ => 0.15m };
    internal static string Currency(string country) => country switch { "US" => "USD", "CA" => "CAD", "GB" => "GBP", "AU" => "AUD", "JP" => "JPY", _ => "EUR" };
    internal static string[] StatusPath(string status) => status switch
    {
        "placed" => ["placed"], "cancelled" => ["placed","cancelled"], "paid" => ["placed","paid"],
        "shipped" => ["placed","paid","shipped"], "returned" => ["placed","paid","shipped","delivered","returned"],
        _ => ["placed","paid","shipped","delivered"]
    };

    public EntityIdRange IdRange(string table) { long max=MaximumIds()[table]; return new(max==0?0:1,max); }

    public Dictionary<string,long> MaximumIds() => new(StringComparer.Ordinal)
    {
        ["warehouses"]=20, ["categories"]=400, ["suppliers"]=Suppliers, ["customers"]=Customers, ["customer_addresses"]=Addresses,
        ["products"]=Products, ["inventory"]=Products, ["orders"]=Orders, ["order_items"]=Orders,
        ["payments"]=LastOrder(false)*2, ["shipments"]=LastOrder(true), ["reviews"]=Reviews, ["order_status_history"]=Orders
    };

    private long LastOrder(bool shipment)
    {
        for(long id=Orders;id>0;id--)
        {
            string status=Order(id).Status;
            if(shipment ? status is "shipped" or "delivered" or "returned" : status is not "placed" and not "cancelled") return id;
        }
        return 0;
    }
}
