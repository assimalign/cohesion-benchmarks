using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using MongoDB.Bson;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal static class DocumentRows
{
    internal static string Key(JsonElement doc) => doc.GetProperty("id").GetString()!;
    internal static DateTimeOffset Date(string value) => DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    internal static void Order(JsonElement doc, IVerificationSink sink)
    {
        string customer = doc.TryGetProperty("customerId", out var c) ? c.GetString()! : doc.GetProperty("customer").GetProperty("id").GetString()!;
        sink.AddRow([Key(doc), customer, Date(doc.GetProperty("placedAt").GetString()!), doc.GetProperty("status").GetString(), doc.GetProperty("total").GetDecimal()]);
    }
    internal static void Product(JsonElement doc, IVerificationSink sink)
    {
        decimal price = doc.TryGetProperty("price", out var p) ? p.GetDecimal() : doc.GetProperty("variants")[0].GetProperty("price").GetDecimal();
        sink.AddRow([Key(doc), doc.GetProperty("name").GetString(), doc.GetProperty("category").GetString(), price]);
    }
    internal static void Mutation(string operation, JsonElement doc, IVerificationSink sink)
    {
        switch (operation)
        {
            case "doc.insert-order": Order(doc, sink); break;
            case "doc.patch-order-status":
                var history = doc.GetProperty("statusHistory"); var last = history[history.GetArrayLength() - 1];
                sink.AddRow([Key(doc), doc.GetProperty("status").GetString(), (long)history.GetArrayLength(), last.GetProperty("status").GetString(), Date(last.GetProperty("occurredAt").GetString()!)]); break;
            case "doc.adjust-variant-stock":
                var variant = doc.GetProperty("variants")[0];
                sink.AddRow([Key(doc), 0L, variant.GetProperty("sku").GetString(), variant.GetProperty("stock").GetInt64()]); break;
            case "doc.replace-customer":
                // Canonical scalar leaves of the entire replacement prove that nested data was replaced too.
                Flatten(doc, "", sink); break;
        }
    }
    private static void Flatten(JsonElement value, string path, IVerificationSink sink)
    {
        if (value.ValueKind == JsonValueKind.Object)
        {
            var properties = new List<JsonProperty>();
            foreach (var property in value.EnumerateObject())
                if (property.Name != "@metadata" && !property.Name.StartsWith('_')) properties.Add(property);
            properties.Sort((x, y) => StringComparer.Ordinal.Compare(x.Name, y.Name));
            foreach (var property in properties)
            {
                if (path.Length == 0 && property.Name == "id") sink.AddRow(["/id", Key(value)]);
                else Flatten(property.Value, path + "/" + property.Name, sink);
            }
        }
        else if (value.ValueKind == JsonValueKind.Array)
        {
            sink.AddRow([path + "/length", (long)value.GetArrayLength()]); int index = 0;
            foreach (var item in value.EnumerateArray()) Flatten(item, path + "/" + (index++).ToString(CultureInfo.InvariantCulture), sink);
        }
        else
        {
            object? scalar = value.ValueKind switch { JsonValueKind.String => value.GetString(), JsonValueKind.Number => value.GetDecimal(),
                JsonValueKind.True => true, JsonValueKind.False => false, _ => null };
            sink.AddRow([path, scalar]);
        }
    }
    internal static void AddItems(JsonElement items, SortedDictionary<string, decimal> totals)
    {
        foreach (var item in items.EnumerateArray())
        {
            string category = item.GetProperty("category").GetString()!;
            totals.TryGetValue(category, out decimal previous);
            totals[category] = previous + item.GetProperty("quantity").GetDecimal() * item.GetProperty("price").GetDecimal();
        }
    }
    internal static void Revenue(SortedDictionary<string, decimal> totals, IVerificationSink? sink)
    { if (sink != null) foreach (var pair in totals) sink.AddRow([pair.Key, pair.Value]); }
    internal static byte[] MongoBytes(BsonDocument doc)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(buffer)) WriteBson(writer, doc);
        return buffer.WrittenSpan.ToArray();
    }
    internal static JsonDocument MongoJson(BsonDocument doc) => JsonDocument.Parse(MongoBytes(doc));
    private static void WriteBson(Utf8JsonWriter writer, BsonValue value)
    {
        switch (value.BsonType)
        {
            case BsonType.Document:
                writer.WriteStartObject(); foreach (var field in value.AsBsonDocument) { writer.WritePropertyName(field.Name); WriteBson(writer, field.Value); } writer.WriteEndObject(); break;
            case BsonType.Array:
                writer.WriteStartArray(); foreach (var item in value.AsBsonArray) WriteBson(writer, item); writer.WriteEndArray(); break;
            case BsonType.String: writer.WriteStringValue(value.AsString); break;
            case BsonType.Boolean: writer.WriteBooleanValue(value.AsBoolean); break;
            case BsonType.Int32: writer.WriteNumberValue(value.AsInt32); break;
            case BsonType.Int64: writer.WriteNumberValue(value.AsInt64); break;
            case BsonType.Decimal128: writer.WriteNumberValue(Decimal128.ToDecimal(value.AsDecimal128)); break;
            case BsonType.Double: writer.WriteNumberValue(value.AsDouble); break;
            case BsonType.Null: writer.WriteNullValue(); break;
            default: throw new InvalidOperationException("Unexpected benchmark BSON type: " + value.BsonType);
        }
    }
}
