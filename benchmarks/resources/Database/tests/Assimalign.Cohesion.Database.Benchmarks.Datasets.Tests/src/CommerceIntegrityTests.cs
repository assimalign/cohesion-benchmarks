using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Benchmarks.Datasets;
using Shouldly;
using Xunit;
using static Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests.DatasetRun;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests;

/// <summary>Full SF0.01 relational integrity, lifecycle, totals and descriptor checks.</summary>
public sealed class CommerceIntegrityTests
{
    [Fact]
    public async Task Generate_CommercePointZeroOne_SatisfiesSchemaReferencesAndTotals()
    {
        using var run=await Generate(DatasetModel.Sql);
        var descriptor=new CommerceDescriptor(run.Manifest);
        var tables=new Dictionary<string,List<Dictionary<string,string?>>>(StringComparer.Ordinal);
        foreach(var table in CommerceSchema.Tables)
        {
            var rows=run.Csv(table.Name).ToList(); tables.Add(table.Name,rows);
            ((long)rows.Count).ShouldBe(run.Manifest.Tables.Single(t=>t.Name==table.Name).Rows);
            var keys=new HashSet<string>(StringComparer.Ordinal);
            foreach(var row in rows)
            {
                keys.Add(string.Join("|",table.PrimaryKey.Select(k=>row[k]))).ShouldBeTrue();
                foreach(var column in table.Columns)
                {
                    string? value=row[column.Name];
                    if(!column.Nullable) value.ShouldNotBeNull();
                    if(value is null) continue;
                    if(column.Length is not null) value.Length.ShouldBeLessThanOrEqualTo(column.Length.Value);
                    if(column.Type==LogicalType.Numeric) decimal.Parse(value,CultureInfo.InvariantCulture).ShouldBe(decimal.Round(decimal.Parse(value,CultureInfo.InvariantCulture),2));
                }
            }
        }
        var customers=tables["customers"].ToDictionary(r=>Long(r,"customer_id"));
        var addresses=tables["customer_addresses"].ToDictionary(r=>Long(r,"address_id"));
        var products=tables["products"].ToDictionary(r=>Long(r,"product_id"));
        var orders=tables["orders"].ToDictionary(r=>Long(r,"order_id"));
        var categories=tables["categories"].ToDictionary(r=>Long(r,"category_id"));
        var suppliers=tables["suppliers"].Select(r=>Long(r,"supplier_id")).ToHashSet();
        var warehouses=tables["warehouses"].Select(r=>Long(r,"warehouse_id")).ToHashSet();
        customers.Count.ShouldBe(2500); addresses.Count.ShouldBe(3750); products.Count.ShouldBe(500); orders.Count.ShouldBe(10000);
        categories.Count.ShouldBe(400); warehouses.Count.ShouldBe(20); suppliers.Count.ShouldBe(20);
        customers.Values.Select(r=>r["email"]).Distinct().Count().ShouldBe(customers.Count);
        products.Values.Select(r=>r["sku"]).Distinct().Count().ShouldBe(products.Count);

        foreach(var (id,row) in categories)
        {
            row["name"].ShouldBe(CommerceDescriptor.CategoryName(id));
            long? parent=row["parent_id"] is null?null:Long(row,"parent_id");
            parent.ShouldBe(CommerceDescriptor.CategoryParent(id));
            int depth=1; long? current=parent;
            while(current is not null) { categories.ContainsKey(current.Value).ShouldBeTrue(); depth++; depth.ShouldBeLessThanOrEqualTo(3); current=CommerceDescriptor.CategoryParent(current.Value); }
        }
        foreach(var (id,row) in customers)
        {
            row["email"].ShouldBe(descriptor.CustomerEmail(id));
            row["first_name"].ShouldBe(descriptor.Customer(id).First);
            descriptor.IdRange("customers").Contains(id).ShouldBeTrue();
        }
        foreach(var (id,row) in addresses)
        {
            customers.ContainsKey(Long(row,"customer_id")).ShouldBeTrue();
            Long(row,"customer_id").ShouldBe(descriptor.AddressCustomer(id));
        }
        foreach(var (id,row) in products)
        {
            categories.ContainsKey(Long(row,"category_id")).ShouldBeTrue(); suppliers.Contains(Long(row,"supplier_id")).ShouldBeTrue();
            Long(row,"category_id").ShouldBe(descriptor.ProductCategory(id)); row["brand"].ShouldBe(descriptor.ProductBrand(id));
            Money(row,"unit_price").ShouldBe(descriptor.Product(id).Price);
            row["description"]!.Length.ShouldBeInRange(50,1000);
        }
        foreach(var row in tables["inventory"])
        {
            warehouses.Contains(Long(row,"warehouse_id")).ShouldBeTrue(); products.ContainsKey(Long(row,"product_id")).ShouldBeTrue();
        }
        var subtotals=new Dictionary<long,decimal>(); var lines=new Dictionary<long,int>();
        foreach(var row in tables["order_items"])
        {
            long order=Long(row,"order_id"),product=Long(row,"product_id"); int line=(int)Long(row,"line_no");
            orders.ContainsKey(order).ShouldBeTrue(); products.ContainsKey(product).ShouldBeTrue();
            Money(row,"unit_price").ShouldBe(Money(products[product],"unit_price"));
            Money(row,"line_total").ShouldBe(Long(row,"quantity")*Money(row,"unit_price")-Money(row,"discount"));
            var recomputed=descriptor.Line(order,line); product.ShouldBe(recomputed.Product); Money(row,"line_total").ShouldBe(recomputed.Total);
            subtotals[order]=subtotals.GetValueOrDefault(order)+Money(row,"line_total"); lines[order]=lines.GetValueOrDefault(order)+1;
        }
        foreach(var (id,row) in orders)
        {
            long customer=Long(row,"customer_id"); customers.ContainsKey(customer).ShouldBeTrue();
            long address=Long(row,"shipping_address_id"); addresses.ContainsKey(address).ShouldBeTrue();
            Long(addresses[address],"customer_id").ShouldBe(customer);
            Time(row,"order_date").ShouldBe(descriptor.OrderDate(id));
            var o=descriptor.Order(id); customer.ShouldBe(o.Customer); address.ShouldBe(o.Address); row["status"].ShouldBe(o.Status); lines[id].ShouldBe(o.Lines);
            Money(row,"subtotal").ShouldBe(subtotals[id]);
            decimal rate=customers[customer]["country_code"] switch { "US"=>0.07m,"CA"=>0.13m,"GB"=>0.20m,"DE"=>0.19m,"FR"=>0.20m,"AU"=>0.10m,"JP"=>0.10m,_=>0.15m };
            Money(row,"tax").ShouldBe(decimal.Round(subtotals[id]*rate,2,MidpointRounding.AwayFromZero));
            Money(row,"total").ShouldBe(Money(row,"subtotal")+Money(row,"tax")+Money(row,"shipping_fee"));
        }
        var paid=new HashSet<long>(); var shipped=new HashSet<long>(); var attempts=new Dictionary<long,List<Dictionary<string,string?>>>();
        foreach(var row in tables["payments"])
        {
            long order=Long(row,"order_id"); orders.ContainsKey(order).ShouldBeTrue();
            Money(row,"amount").ShouldBe(Money(orders[order],"total"));
            Time(row,"paid_at").ShouldBeGreaterThan(Time(orders[order],"order_date"));
            if(!attempts.TryGetValue(order,out var list)) attempts[order]=list=new(); list.Add(row);
            if(row["status"]=="succeeded") paid.Add(order).ShouldBeTrue();
        }
        foreach(var list in attempts.Values)
            if(list.Count==2) { list[0]["status"].ShouldBe("failed"); Time(list[0],"paid_at").ShouldBeLessThan(Time(list[1],"paid_at")); }
        foreach(var row in tables["shipments"])
        {
            long order=Long(row,"order_id"); orders.ContainsKey(order).ShouldBeTrue(); paid.Contains(order).ShouldBeTrue();
            warehouses.Contains(Long(row,"warehouse_id")).ShouldBeTrue(); shipped.Add(order).ShouldBeTrue();
            Time(row,"shipped_at").ShouldBeGreaterThan(Time(orders[order],"order_date"));
            if(orders[order]["status"]=="shipped") row["delivered_at"].ShouldBeNull();
            else Time(row,"delivered_at").ShouldBeGreaterThan(Time(row,"shipped_at"));
        }
        var histories=tables["order_status_history"].GroupBy(r=>Long(r,"order_id")).ToDictionary(g=>g.Key,g=>g.OrderBy(r=>Long(r,"seq")).ToArray());
        foreach(var (id,row) in orders)
        {
            string status=row["status"]!;
            paid.Contains(id).ShouldBe(status is not "placed" and not "cancelled");
            shipped.Contains(id).ShouldBe(status is "shipped" or "delivered" or "returned");
            var history=histories[id]; history[0]["status"].ShouldBe("placed"); history[^1]["status"].ShouldBe(status);
            Time(history[0],"occurred_at").ShouldBe(Time(row,"order_date"));
            for(int i=0;i<history.Length;i++)
            {
                Long(history[i],"seq").ShouldBe(i+1); Time(history[i],"occurred_at").ShouldBeLessThanOrEqualTo(DatasetTime.End);
                if(i>0) Time(history[i],"occurred_at").ShouldBeGreaterThan(Time(history[i-1],"occurred_at"));
            }
        }
        foreach(var row in tables["reviews"])
        {
            products.ContainsKey(Long(row,"product_id")).ShouldBeTrue(); customers.ContainsKey(Long(row,"customer_id")).ShouldBeTrue();
            Long(row,"rating").ShouldBeInRange(1,5); row["body"]!.Length.ShouldBeInRange(100,2000);
        }
        foreach(var table in run.Manifest.Tables)
        {
            string id=table.Name switch { "inventory"=>"product_id","order_items" or "order_status_history"=>"order_id",_=>CommerceSchema.Table(table.Name).PrimaryKey[0] };
            long maximum=tables[table.Name].Count==0?0:tables[table.Name].Max(r=>Long(r,id));
            table.MaximumId.ShouldBe(maximum); descriptor.MaximumIds()[table.Name].ShouldBe(maximum);
        }
        double cancelled=orders.Values.Count(r=>r["status"]=="cancelled")/(double)orders.Count;
        cancelled.ShouldBeInRange(0.02,0.04);
        tables["reviews"].Average(r=>Long(r,"rating")).ShouldBeGreaterThan(4.0);
    }
}
