using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Streaming commerce tables. Every order range writes its five dependent tables in one pass.</summary>
public sealed class CommerceGenerator : IDatasetGenerator
{
    public DatasetModel Model => DatasetModel.Sql;
    public string Name => "commerce";

    public async ValueTask<DatasetManifest> GenerateAsync(DatasetGenerationOptions options, IProgress<DatasetProgress>? progress, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var session = new GenerationSession(options,Model,progress);
        var values = new CommerceDescriptor(options.Seed,options.Scale);
        await Table("warehouses",20,200000, (w,id) => {
            City city = DatasetText.Cities[(id-1)%DatasetText.Cities.Length];
            w.WriteRow(id,"Warehouse "+id,city.Name,city.CountryCode);
        });
        await Table("categories",400,200000,(w,id) => w.WriteRow(id,CommerceDescriptor.CategoryParent(id),CommerceDescriptor.CategoryName(id)));
        await Table("suppliers",values.Suppliers,200000,(w,id) => {
            var r = values.Random("commerce.supplier",id);
            w.WriteRow(id,DatasetText.Pick(DatasetText.Brands,ref r)+" Supply "+id,DatasetText.Cities[r.Int(0,DatasetText.Cities.Length-1)].CountryCode,r.Decimal(2.5m,5));
        });
        await Table("customers",values.Customers,500000,(w,id) => {
            var c = values.Customer(id); var r = values.Random("commerce.customer.details",id);
            w.WriteRow(id,values.CustomerEmail(id),c.First,c.Last,r.Chance(0.08)?null:"+1-555-"+r.Int(1000000,9999999),
                new DateOnly(1950,1,1).AddDays(r.Int(0,20000)),c.City.CountryCode,c.City.Name,DatasetTime.Sample(ref r),
                r.Chance(0.1)?"gold":r.Chance(0.3)?"silver":"standard",r.Chance(0.62),r.Decimal(0,25000));
        });
        await Table("customer_addresses",values.Addresses,1000000,(w,id) => {
            long customer = values.AddressCustomer(id); var c = values.Customer(customer); var r = values.Random("commerce.address",id);
            w.WriteRow(id,customer,id<=values.Customers?"home":"work",r.Int(1,9999)+" "+DatasetText.Pick(DatasetText.LastNames,ref r)+" Street",
                r.Chance(0.2)?"Unit "+r.Int(1,200):null,c.City.Name,c.City.Name+" Region",r.Int(10000,99999).ToString(System.Globalization.CultureInfo.InvariantCulture),c.City.CountryCode,id<=values.Customers);
        });
        await Table("products",values.Products,200000,(w,id) => {
            var p = values.Product(id); var r = values.Random("commerce.product.details",id);
            w.WriteRow(id,"SKU-"+id.ToString("D10",System.Globalization.CultureInfo.InvariantCulture),p.Name,
                DatasetText.Paragraph(ref r,50,1000,230),values.ProductCategory(id),p.Supplier,values.ProductBrand(id),p.Price,
                CommerceDescriptor.Money(p.Price*r.Decimal(0.35m,0.8m)),r.Int(20,15000),DatasetTime.Sample(ref r),r.Chance(0.96));
        });
        await session.Ranges(values.Products,500000,(part,start,end) => {
            using var output = session.Csv("inventory",part,CommerceSchema.Table("inventory").Header());
            for(long id=start;id<=end;id++) {
                Check(id,cancellationToken); var r=values.Random("commerce.inventory",id);
                for(int slot=0;slot<4;slot++) output.Csv.WriteRow(1+(id+slot*5)%20,id,r.Int(0,2000),r.Int(10,150),DatasetTime.Sample(ref r));
            }
        },cancellationToken);
        await session.Ranges(values.Orders,1000000,(part,start,end) => WriteOrders(session,values,part,start,end,cancellationToken),cancellationToken);
        await Table("reviews",values.Reviews,150000,(w,id) => {
            var r = values.Random("commerce.review",id);
            w.WriteRow(id,values.PopularProduct(ref r),values.PopularCustomer(ref r),CommerceDescriptor.Ratings.Sample(ref r)+1,
                DatasetText.Paragraph(ref r,10,100,32),DatasetText.Paragraph(ref r,100,2000,650),DatasetTime.Sample(ref r),r.Int(0,200));
        });
        return session.Finish(Name,CommerceSchema.LoadOrder,values.MaximumIds());

        Task Table(string table,long count,int perPart,Action<CsvWriter,long> write) => session.Ranges(count,perPart,(part,start,end) => {
            using var output=session.Csv(table,part,CommerceSchema.Table(table).Header());
            for(long id=start;id<=end;id++) { Check(id,cancellationToken); write(output.Csv,id); }
        },cancellationToken);
    }

    private static void Check(long id,CancellationToken token) { if ((id & 255)==0) token.ThrowIfCancellationRequested(); }

    private static void WriteOrders(GenerationSession session,CommerceDescriptor values,int part,long start,long end,CancellationToken token)
    {
        using var orders=session.Csv("orders",part,CommerceSchema.Table("orders").Header());
        using var items=session.Csv("order_items",part,CommerceSchema.Table("order_items").Header());
        using var payments=session.Csv("payments",part,CommerceSchema.Table("payments").Header());
        using var shipments=session.Csv("shipments",part,CommerceSchema.Table("shipments").Header());
        using var history=session.Csv("order_status_history",part,CommerceSchema.Table("order_status_history").Header());
        for(long id=start;id<=end;id++)
        {
            Check(id,token);
            var o=values.Order(id); var c=values.Customer(o.Customer); var r=values.Random("commerce.order.details",id);
            decimal subtotal=0;
            for(int line=1;line<=o.Lines;line++)
            {
                var item=values.Line(id,line); subtotal+=item.Total;
                items.Csv.WriteRow(id,line,item.Product,item.Quantity,item.Price,item.Discount,item.Total);
            }
            decimal tax=CommerceDescriptor.Money(subtotal*CommerceDescriptor.TaxRate(c.City.CountryCode)), shipping=subtotal>=100?0:6.50m;
            decimal total=subtotal+tax+shipping;
            orders.Csv.WriteRow(id,o.Customer,o.Date,o.Status,o.Channel,o.Address,CommerceDescriptor.Currency(c.City.CountryCode),subtotal,tax,shipping,total,
                r.Chance(0.12)?"SAVE10":null);
            string[] path=CommerceDescriptor.StatusPath(o.Status);
            DateTime occurred=o.Date;
            DateTime? shipped=null,delivered=null;
            for(int seq=0;seq<path.Length;seq++)
            {
                if(seq!=0) occurred=DatasetTime.After(seq==1?occurred.AddSeconds(1):occurred,ref r,seq==1?1800:172800,path.Length-seq+3);
                history.Csv.WriteRow(id,seq+1,path[seq],occurred,r.Chance(0.04)?"Customer notification recorded":null);
                if(path[seq]=="paid")
                {
                    if(r.Chance(0.08)) payments.Csv.WriteRow(id*2-1,id,"card",total,"failed",o.Date.AddSeconds(1),"pay-failed-"+id);
                    payments.Csv.WriteRow(id*2,id,r.Chance(0.75)?"card":"wallet",total,"succeeded",occurred,"pay-"+id);
                }
                if(path[seq]=="shipped") shipped=occurred;
                if(path[seq]=="delivered") delivered=occurred;
            }
            if(shipped is not null) shipments.Csv.WriteRow(id,id,r.Int(1,20),r.Chance(0.5)?"Parcel North":"Postal Express","TRACK-"+id,shipped,delivered);
        }
    }
}
