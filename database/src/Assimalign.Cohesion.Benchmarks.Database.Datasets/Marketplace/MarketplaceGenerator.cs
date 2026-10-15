using System;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Heterogeneous marketplace documents, with embedded values recomputed from their source entities.</summary>
public sealed class MarketplaceGenerator : IDatasetGenerator
{
    public DatasetModel Model => DatasetModel.Documents;
    public string Name => "marketplace";

    public async ValueTask<DatasetManifest> GenerateAsync(DatasetGenerationOptions options,IProgress<DatasetProgress>? progress,CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var session=new GenerationSession(options,Model,progress);
        var values=new MarketplaceDescriptor(options.Seed,options.Scale);
        await Collection("customers",values.Customers,100000,WriteCustomer);
        await Collection("products",values.Products,40000,WriteProduct);
        await Collection("orders",values.Orders,60000,WriteOrder);
        await Collection("reviews",values.Reviews,100000,WriteReview);
        return session.Finish(Name,MarketplaceSchema.LoadOrder,values.MaximumIds());

        Task Collection(string name,long count,int perPart,Action<Utf8JsonWriter,MarketplaceDescriptor,long> write) =>
            session.Ranges(count,perPart,(part,start,end) => {
                using var output=session.Json(name,part);
                for(long id=start;id<=end;id++)
                {
                    if((id&255)==0) cancellationToken.ThrowIfCancellationRequested();
                    Utf8JsonWriter w=output.Json.BeginDocument();
                    w.WriteString("id",MarketplaceDescriptor.Key(name,id));
                    write(w,values,id); output.Json.EndDocument();
                }
            },cancellationToken);
    }

    private static void Snapshot(Utf8JsonWriter w,MarketplaceDescriptor v,long id)
    {
        var c=v.Customer(id);
        w.WriteString("id",MarketplaceDescriptor.Key("customers",id)); w.WriteString("firstName",c.First);
        w.WriteString("lastName",c.Last); w.WriteString("email",c.Email);
    }

    private static void Address(Utf8JsonWriter w,MarketplaceDescriptor v,long id,int number)
    {
        var c=v.Customer(id); var r=v.Random("address."+number,id);
        w.WriteStartObject();
        w.WriteString("kind",number==0?"home":"work");
        w.WriteString("line1",r.Int(1,9999)+" "+DatasetText.Pick(DatasetText.LastNames,ref r)+" Street");
        if(r.Chance(0.2)) w.WriteString("line2","Unit "+r.Int(1,100));
        w.WriteString("city",c.City.Name); w.WriteString("region",c.City.Name+" Region");
        w.WriteString("postalCode",r.Int(10000,99999).ToString(CultureInfo.InvariantCulture)); w.WriteString("country",c.City.CountryCode);
        w.WriteBoolean("isDefault",number==0); w.WriteEndObject();
    }

    private static void WriteCustomer(Utf8JsonWriter w,MarketplaceDescriptor v,long id)
    {
        var c=v.Customer(id); var r=v.Random("customer.details",id);
        w.WriteString("firstName",c.First); w.WriteString("lastName",c.Last); w.WriteString("email",c.Email);
        if(r.Chance(0.9)) w.WriteString("phone","+1-555-"+r.Int(1000000,9999999));
        w.WriteString("birthDate",new DateOnly(1950,1,1).AddDays(r.Int(0,20000)).ToString("yyyy-MM-dd",CultureInfo.InvariantCulture));
        JsonLinesWriter.Timestamp(w,"createdAt",DatasetTime.Sample(ref r));
        w.WriteStartArray("addresses"); Address(w,v,id,0); if(r.Chance(0.5)) Address(w,v,id,1); w.WriteEndArray();
        w.WriteStartObject("preferences"); Strings(w,"categories",MarketplaceDescriptor.Categories,ref r,2);
        w.WriteString("locale","en"); w.WriteString("currency",CommerceDescriptor.Currency(c.City.CountryCode));
        w.WriteBoolean("emailReceipts",r.Chance(0.9)); w.WriteBoolean("compactView",r.Chance(0.3));
        Strings(w,"interests",DatasetText.Topics,ref r,4); w.WriteEndObject();
        w.WriteStartArray("devices"); int devices=r.Int(1,3);
        for(int i=0;i<devices;i++) {
            w.WriteStartObject(); w.WriteString("id","device-"+id+"-"+i);
            w.WriteString("type",i==0?"phone":"desktop"); w.WriteString("platform",r.Chance(0.5)?"browser":"mobile");
            JsonLinesWriter.Timestamp(w,"lastSeenAt",DatasetTime.Sample(ref r)); w.WriteEndObject();
        }
        w.WriteEndArray();
        w.WriteStartObject("loyalty"); w.WriteString("tier",r.Chance(0.2)?"gold":"standard");
        w.WriteNumber("points",r.Int(0,20000)); JsonLinesWriter.Money(w,"lifetimeValue",r.Decimal(0,25000)); w.WriteEndObject();
        w.WriteStartObject("consent"); w.WriteBoolean("marketing",r.Chance(0.62)); w.WriteBoolean("analytics",r.Chance(0.75));
        w.WriteBoolean("personalization",r.Chance(0.8)); w.WriteEndObject();
    }

    private static void WriteProduct(Utf8JsonWriter w,MarketplaceDescriptor v,long id)
    {
        var p=v.Product(id); var r=v.Random("product.details",id);
        w.WriteString("name",p.Name); w.WriteString("category",p.Category); w.WriteString("brand",DatasetText.Pick(DatasetText.Brands,ref r));
        w.WriteString("description",DatasetText.Paragraph(ref r,300,12000,1350));
        JsonLinesWriter.Timestamp(w,"createdAt",DatasetTime.Sample(ref r)); w.WriteBoolean("active",r.Chance(0.96));
        switch(p.Category)
        {
            case "electronics":
                w.WriteStartObject("specs"); w.WriteString("processor",r.Chance(0.5)?"ARM":"x64");
                w.WriteNumber("memoryGb",1<<r.Int(2,6)); w.WriteNumber("storageGb",1<<r.Int(6,11));
                w.WriteNumber("powerWatts",r.Int(5,250)); w.WriteBoolean("wireless",r.Chance(0.8)); w.WriteEndObject();
                if(r.Chance(0.7)) w.WriteNumber("warrantyMonths",r.Int(1,3)*12);
                break;
            case "apparel":
                Strings(w,"sizes",["XS","S","M","L","XL"],ref r,4); Strings(w,"colors",["blue","red","black","white","green"],ref r,3);
                w.WriteString("material",r.Chance(0.7)?"cotton":"wool"); break;
            case "books":
                w.WriteStartArray("authors"); int authors=r.Int(1,3);
                for(int i=0;i<authors;i++) w.WriteStringValue(DatasetText.Pick(DatasetText.FirstNames,ref r)+" "+DatasetText.Pick(DatasetText.LastNames,ref r));
                w.WriteEndArray(); w.WriteString("isbn",Isbn(id)); w.WriteNumber("pages",r.Int(80,900)); break;
            case "grocery":
                w.WriteStartObject("nutrition"); w.WriteNumber("servingGrams",r.Int(20,150)); w.WriteNumber("calories",r.Int(20,600));
                JsonLinesWriter.Money(w,"proteinGrams",r.Decimal(0,30)); JsonLinesWriter.Money(w,"fatGrams",r.Decimal(0,40));
                JsonLinesWriter.Money(w,"carbohydrateGrams",r.Decimal(0,80)); w.WriteEndObject();
                w.WriteBoolean("organic",r.Chance(0.3)); break;
        }
        w.WriteStartArray("variants");
        for(int i=0;i<p.Variants;i++) {
            var variant=v.Variant(id,i); w.WriteStartObject(); w.WriteString("sku",variant.Sku);
            JsonLinesWriter.Money(w,"price",variant.Price); w.WriteNumber("stock",variant.Stock);
            w.WriteStartObject("attributes"); w.WriteString("edition","edition-"+(i+1));
            if(p.Category=="apparel") w.WriteString("size",new[]{"S","M","L","XL"}[i]);
            else w.WriteString("packaging",i==0?"standard":"extended");
            w.WriteEndObject(); w.WriteEndObject();
        }
        w.WriteEndArray(); Strings(w,"tags",DatasetText.Topics,ref r,r.Int(3,7));
        w.WriteStartObject("rating"); JsonLinesWriter.Money(w,"average",r.Decimal(3.5m,5)); w.WriteNumber("count",r.Int(0,2000)); w.WriteEndObject();
        w.WriteString("seller",MarketplaceDescriptor.Key("customers",r.Long(1,v.Customers)));
        w.WriteStartArray("images"); int images=r.Int(2,5);
        for(int i=0;i<images;i++) w.WriteStringValue("https://images.example.test/products/"+id+"/"+i+".jpg");
        w.WriteEndArray();
        if(r.Chance(0.3)) w.WriteString("careInstructions",DatasetText.Paragraph(ref r,30,200,70));
    }

    private static void WriteOrder(Utf8JsonWriter w,MarketplaceDescriptor v,long id)
    {
        var o=v.Order(id); var c=v.Customer(o.Customer); var r=v.Random("order.details",id);
        w.WriteStartObject("customer"); Snapshot(w,v,o.Customer); w.WriteEndObject();
        w.WritePropertyName("shippingAddress"); Address(w,v,o.Customer,0);
        w.WriteStartArray("items"); decimal subtotal=0;
        for(int i=0;i<o.Lines;i++)
        {
            long product=v.PopularProduct(ref r); var p=v.Product(product); var variant=v.Variant(product,r.Int(0,p.Variants-1));
            int quantity=r.Int(1,3); decimal lineTotal=quantity*variant.Price; subtotal+=lineTotal;
            w.WriteStartObject(); w.WriteString("productId",MarketplaceDescriptor.Key("products",product));
            w.WriteString("name",p.Name); w.WriteString("category",p.Category); w.WriteString("variant",variant.Sku);
            w.WriteNumber("quantity",quantity); JsonLinesWriter.Money(w,"price",variant.Price); JsonLinesWriter.Money(w,"lineTotal",lineTotal); w.WriteEndObject();
        }
        w.WriteEndArray();
        decimal tax=CommerceDescriptor.Money(subtotal*CommerceDescriptor.TaxRate(c.City.CountryCode)),shipping=subtotal>=100?0:6.50m,total=subtotal+tax+shipping;
        w.WriteStartObject("payment"); w.WriteString("method",r.Chance(0.75)?"card":"wallet");
        w.WriteString("status",o.Status is "placed" or "cancelled"?"pending":"succeeded");
        JsonLinesWriter.Money(w,"amount",total); if(o.Status is not "placed" and not "cancelled") w.WriteString("reference","mp-pay-"+id);
        w.WriteEndObject();
        w.WriteStartArray("statusHistory"); string[] path=CommerceDescriptor.StatusPath(o.Status); DateTime time=o.Date;
        for(int i=0;i<path.Length;i++) {
            if(i>0) time=DatasetTime.After(time,ref r,i==1?1800:172800,path.Length-i);
            w.WriteStartObject(); w.WriteString("status",path[i]); JsonLinesWriter.Timestamp(w,"occurredAt",time); w.WriteEndObject();
        }
        w.WriteEndArray(); w.WriteString("currency",CommerceDescriptor.Currency(c.City.CountryCode));
        JsonLinesWriter.Money(w,"subtotal",subtotal); JsonLinesWriter.Money(w,"tax",tax);
        JsonLinesWriter.Money(w,"shippingFee",shipping); JsonLinesWriter.Money(w,"total",total);
        JsonLinesWriter.Timestamp(w,"placedAt",o.Date); w.WriteString("status",o.Status);
        if(r.Chance(0.12)) w.WriteString("promoCode","SAVE10");
        if(r.Chance(0.07)) w.WriteString("note",DatasetText.Paragraph(ref r,20,250,60));
    }

    private static void WriteReview(Utf8JsonWriter w,MarketplaceDescriptor v,long id)
    {
        var r=v.Random("review",id);
        w.WriteString("productId",MarketplaceDescriptor.Key("products",v.PopularProduct(ref r)));
        w.WriteString("customerId",MarketplaceDescriptor.Key("customers",v.PopularCustomer(ref r)));
        w.WriteNumber("rating",CommerceDescriptor.Ratings.Sample(ref r)+1);
        if(r.Chance(0.95)) w.WriteString("title",DatasetText.Paragraph(ref r,10,100,30));
        w.WriteString("body",DatasetText.Paragraph(ref r,100,2000,300));
        DateTime time=DatasetTime.Sample(ref r,20); JsonLinesWriter.Timestamp(w,"createdAt",time);
        w.WriteNumber("helpfulVotes",r.Int(0,300)); w.WriteStartArray("media");
        if(r.Chance(0.15)) w.WriteStringValue("https://images.example.test/reviews/"+id+".jpg");
        w.WriteEndArray(); w.WriteStartArray("replies"); int replies=r.Chance(0.22)?r.Int(1,3):0;
        for(int i=0;i<replies;i++) {
            time=DatasetTime.After(time,ref r,604800,replies-i);
            w.WriteStartObject(); w.WriteString("customerId",MarketplaceDescriptor.Key("customers",r.Long(1,v.Customers)));
            w.WriteString("body",DatasetText.Paragraph(ref r,20,400,70)); JsonLinesWriter.Timestamp(w,"createdAt",time); w.WriteEndObject();
        }
        w.WriteEndArray();
    }

    private static void Strings(Utf8JsonWriter w,string name,string[] choices,ref EntityRandom r,int count)
    {
        // Rotate through a vocabulary, avoiding duplicates without allocating a set.
        int offset=r.Int(0,choices.Length-1); w.WriteStartArray(name);
        for(int i=0;i<Math.Min(count,choices.Length);i++) w.WriteStringValue(choices[(offset+i)%choices.Length]);
        w.WriteEndArray();
    }

    private static string Isbn(long id)
    {
        string digits="978"+(id%1000000000).ToString("D9",CultureInfo.InvariantCulture); int sum=0;
        for(int i=0;i<digits.Length;i++) sum+=(digits[i]-'0')*(i%2==0?1:3);
        return digits+((10-sum%10)%10).ToString(CultureInfo.InvariantCulture);
    }
}
