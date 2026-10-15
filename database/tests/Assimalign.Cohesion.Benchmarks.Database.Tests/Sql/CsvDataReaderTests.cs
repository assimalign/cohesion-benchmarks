using System;
using System.IO;
using System.Text;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Sql;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Sql;

public sealed class CsvDataReaderTests
{
    [Fact]
    public void Read_QuotedEmptyAndNull_PreservesTypesAndValues()
    {
        var table = new SchemaTable("test", [new("id", LogicalType.BigInt), new("value", LogicalType.Text, true),
            new("active", LogicalType.Boolean), new("amount", LogicalType.Numeric)], ["id"], []);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("id,value,active,amount\r\n1,,true,12.34\r\n2,\"\",false,0.01\r\n3,\"a,\"\"b\"\"\r\nc\",true,99.99\r\n"));
        using var reader = new CsvDataReader(stream, table);
        reader.Read().ShouldBeTrue();
        reader.GetInt64(0).ShouldBe(1);
        reader.GetValue(1).ShouldBe(DBNull.Value);
        reader.GetBoolean(2).ShouldBeTrue();
        reader.GetDecimal(3).ShouldBe(12.34m);
        reader.Read().ShouldBeTrue();
        reader.GetString(1).ShouldBe("");
        reader.Read().ShouldBeTrue();
        reader.GetString(1).ShouldBe("a,\"b\"\r\nc");
        reader.Read().ShouldBeFalse();
    }
}
