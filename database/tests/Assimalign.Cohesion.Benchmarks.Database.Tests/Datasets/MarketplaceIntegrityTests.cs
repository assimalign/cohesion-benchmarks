using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets;

/// <summary>Every SF0.01 marketplace key, embedded reference, product shape and total is checked.</summary>
public sealed class MarketplaceIntegrityTests
{
    [Fact]
    public async Task Generate_MarketplacePointZeroOne_PreservesSnapshotsShapesAndReferences()
    {
        using var run=await DatasetRun.Generate(DatasetModel.Documents);
        var descriptor=new MarketplaceDescriptor(run.Manifest);
        var collections=new Dictionary<string,Dictionary<string,JsonElement>>(StringComparer.Ordinal);
        foreach(var collection in MarketplaceSchema.Collections)
        {
            var documents=new Dictionary<string,JsonElement>(StringComparer.Ordinal);
            foreach(JsonDocument doc in run.Json(collection.Name))
            {
                using(doc)
                {
                    JsonElement root=doc.RootElement; string key=root.GetProperty("id").GetString()!;
                    key.StartsWith(collection.Name+"/",StringComparison.Ordinal).ShouldBeTrue();
                    long id=long.Parse(key.AsSpan(collection.Name.Length+1),CultureInfo.InvariantCulture);
                    key.ShouldBe(MarketplaceDescriptor.Key(collection.Name,id));
                    descriptor.IdRange(collection.Name).Contains(id).ShouldBeTrue();
                    documents.TryAdd(key,root.Clone()).ShouldBeTrue();
                    NoNulls(root);
                }
            }
            collections.Add(collection.Name,documents);
            ((long)documents.Count).ShouldBe(run.Manifest.Tables.Single(t=>t.Name==collection.Name).Rows);
            run.Manifest.Tables.Single(t=>t.Name==collection.Name).MaximumId.ShouldBe(descriptor.MaximumIds()[collection.Name]);
        }
        var customers=collections["customers"]; var products=collections["products"]; var orders=collections["orders"];
        customers.Count.ShouldBe(1500); products.Count.ShouldBe(1000); orders.Count.ShouldBe(2500); collections["reviews"].Count.ShouldBe(1500);
        int phones=0; var categories=new HashSet<string>();
        foreach(var (key,customer) in customers)
        {
            long id=Id(key); var recomputed=descriptor.Customer(id);
            customer.GetProperty("email").GetString().ShouldBe(recomputed.Email);
            customer.GetProperty("firstName").GetString().ShouldBe(recomputed.First);
            customer.GetProperty("addresses").GetArrayLength().ShouldBeInRange(1,2);
            customer.GetProperty("preferences").ValueKind.ShouldBe(JsonValueKind.Object);
            customer.GetProperty("devices").GetArrayLength().ShouldBeInRange(1,3);
            if(customer.TryGetProperty("phone",out _)) phones++;
        }
        phones.ShouldBeInRange(1200,1450);
        foreach(var (key,product) in products)
        {
            long id=Id(key); var recomputed=descriptor.Product(id); string category=product.GetProperty("category").GetString()!;
            category.ShouldBe(recomputed.Category); categories.Add(category);
            product.GetProperty("name").GetString().ShouldBe(recomputed.Name);
            customers.ContainsKey(product.GetProperty("seller").GetString()!).ShouldBeTrue();
            switch(category)
            {
                case "electronics": product.GetProperty("specs").ValueKind.ShouldBe(JsonValueKind.Object); product.TryGetProperty("nutrition",out _).ShouldBeFalse(); break;
                case "apparel": product.GetProperty("sizes").GetArrayLength().ShouldBeGreaterThan(0); product.GetProperty("colors").GetArrayLength().ShouldBeGreaterThan(0); break;
                case "books": product.GetProperty("authors").GetArrayLength().ShouldBeGreaterThan(0); product.GetProperty("isbn").GetString()!.Length.ShouldBe(13); break;
                case "grocery": product.GetProperty("nutrition").ValueKind.ShouldBe(JsonValueKind.Object); break;
                default: throw new InvalidOperationException(category);
            }
            int index=0; decimal min=decimal.MaxValue,max=decimal.MinValue;
            foreach(JsonElement variant in product.GetProperty("variants").EnumerateArray())
            {
                var expected=descriptor.Variant(id,index++);
                variant.GetProperty("sku").GetString().ShouldBe(expected.Sku);
                decimal price=variant.GetProperty("price").GetDecimal(); price.ShouldBe(expected.Price);
                min=Math.Min(min,price); max=Math.Max(max,price);
                variant.GetProperty("price").GetRawText().ShouldMatch(@"^\d+\.\d{2}$");
            }
            index.ShouldBe(recomputed.Variants); descriptor.VariantPriceRange(id).ShouldBe((min,max));
        }
        categories.Count.ShouldBe(4);
        foreach(var (key,order) in orders)
        {
            var snapshot=order.GetProperty("customer"); string customerKey=snapshot.GetProperty("id").GetString()!;
            customers.ContainsKey(customerKey).ShouldBeTrue(); var customer=customers[customerKey];
            foreach(string field in new[]{"firstName","lastName","email"}) snapshot.GetProperty(field).GetString().ShouldBe(customer.GetProperty(field).GetString());
            order.GetProperty("shippingAddress").GetRawText().ShouldBe(customer.GetProperty("addresses")[0].GetRawText());
            var recomputed=descriptor.Order(Id(key)); Id(customerKey).ShouldBe(recomputed.Customer);
            order.GetProperty("placedAt").GetDateTime().ShouldBe(recomputed.Date);
            order.GetProperty("status").GetString().ShouldBe(recomputed.Status);
            decimal subtotal=0;
            foreach(var item in order.GetProperty("items").EnumerateArray())
            {
                string productKey=item.GetProperty("productId").GetString()!; products.ContainsKey(productKey).ShouldBeTrue();
                var product=products[productKey]; item.GetProperty("name").GetString().ShouldBe(product.GetProperty("name").GetString());
                item.GetProperty("category").GetString().ShouldBe(product.GetProperty("category").GetString());
                var variant=product.GetProperty("variants").EnumerateArray().Single(v=>v.GetProperty("sku").GetString()==item.GetProperty("variant").GetString());
                item.GetProperty("price").GetDecimal().ShouldBe(variant.GetProperty("price").GetDecimal());
                decimal line=item.GetProperty("quantity").GetInt32()*item.GetProperty("price").GetDecimal();
                item.GetProperty("lineTotal").GetDecimal().ShouldBe(line); subtotal+=line;
            }
            order.GetProperty("subtotal").GetDecimal().ShouldBe(subtotal);
            decimal tax=order.GetProperty("tax").GetDecimal(),shipping=order.GetProperty("shippingFee").GetDecimal();
            decimal rate=order.GetProperty("shippingAddress").GetProperty("country").GetString() switch
                { "US"=>0.07m,"CA"=>0.13m,"GB"=>0.20m,"DE"=>0.19m,"FR"=>0.20m,"AU"=>0.10m,"JP"=>0.10m,_=>0.15m };
            tax.ShouldBe(decimal.Round(subtotal*rate,2,MidpointRounding.AwayFromZero));
            order.GetProperty("total").GetDecimal().ShouldBe(subtotal+tax+shipping);
            order.GetProperty("payment").GetProperty("amount").GetDecimal().ShouldBe(subtotal+tax+shipping);
            var history=order.GetProperty("statusHistory"); DateTime previous=DateTime.MinValue; int sequence=0;
            foreach(var entry in history.EnumerateArray())
            {
                DateTime at=entry.GetProperty("occurredAt").GetDateTime(); at.ShouldBeGreaterThan(previous); previous=at;
                at.ShouldBeLessThanOrEqualTo(DatasetTime.End);
                if(sequence++==0) at.ShouldBe(recomputed.Date);
            }
            history[history.GetArrayLength()-1].GetProperty("status").GetString().ShouldBe(recomputed.Status);
        }
        int replyCount=0;
        foreach(var review in collections["reviews"].Values)
        {
            products.ContainsKey(review.GetProperty("productId").GetString()!).ShouldBeTrue();
            customers.ContainsKey(review.GetProperty("customerId").GetString()!).ShouldBeTrue();
            DateTime previous=review.GetProperty("createdAt").GetDateTime();
            foreach(var reply in review.GetProperty("replies").EnumerateArray())
            {
                customers.ContainsKey(reply.GetProperty("customerId").GetString()!).ShouldBeTrue();
                DateTime at=reply.GetProperty("createdAt").GetDateTime(); at.ShouldBeGreaterThan(previous); previous=at; replyCount++;
            }
        }
        replyCount.ShouldBeGreaterThan(100);
    }

    private static long Id(string key) => long.Parse(key.AsSpan(key.IndexOf('/')+1),CultureInfo.InvariantCulture);
    private static void NoNulls(JsonElement element)
    {
        element.ValueKind.ShouldNotBe(JsonValueKind.Null);
        if(element.ValueKind==JsonValueKind.Object)
        {
            var names=new HashSet<string>(StringComparer.Ordinal);
            foreach(var property in element.EnumerateObject()) { names.Add(property.Name).ShouldBeTrue(); NoNulls(property.Value); }
        }
        else if(element.ValueKind==JsonValueKind.Array) foreach(var value in element.EnumerateArray()) NoNulls(value);
    }
}
