using System;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Sql;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Sql;

public sealed class SchemaTests
{
    [Theory]
    [InlineData(SqlDialect.Cohesion, "COLLATE binary", "BOOLEAN", "TIMESTAMP")]
    [InlineData(SqlDialect.PostgreSql, "COLLATE \"C\"", "BOOLEAN", "TIMESTAMP")]
    [InlineData(SqlDialect.SqlServer, "COLLATE Latin1_General_100_BIN2_UTF8", "BIT", "DATETIME2(0)")]
    public void CreateTable_Commerce_PreservesPortableConstraints(SqlDialect dialect, string collation, string boolean, string timestamp)
    {
        string sql = SqlSchema.CreateTable(CommerceSchema.Table("customers"), dialect);
        sql.ShouldContain(collation);
        sql.ShouldContain("marketing_opt_in " + boolean + " NOT NULL");
        sql.ShouldContain("created_at " + timestamp + " NOT NULL");
        sql.ShouldContain("phone VARCHAR(30) " + collation + " NULL");
        sql.ShouldContain("PRIMARY KEY (customer_id)");
        sql.ShouldNotContain("FOREIGN KEY");
        sql.ShouldNotContain("UNIQUE");
    }

    [Fact]
    public void Identifier_StatementInjection_RejectsInput() => Should.Throw<ArgumentException>(() => SqlSchema.Identifier("x; DROP DATABASE x"));
}
