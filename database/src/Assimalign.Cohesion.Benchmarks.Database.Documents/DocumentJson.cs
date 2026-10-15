using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using MongoDB.Bson;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal static class DocumentJson
{
    internal static string Key(ReadOnlyMemory<byte> bytes)
    { using var json = JsonDocument.Parse(bytes); return json.RootElement.GetProperty("id").GetString()!; }
    internal static string CosmosId(string key) => Convert.ToBase64String(Encoding.UTF8.GetBytes(key)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    internal static byte[] Rewrite(ReadOnlyMemory<byte> bytes, string? key = null, string? ravenCollection = null, bool cosmos = false)
    {
        using var json = JsonDocument.Parse(bytes);
        var buffer = new ArrayBufferWriter<byte>(bytes.Length + 128);
        using (var writer = new Utf8JsonWriter(buffer))
        {
            writer.WriteStartObject();
            string identity = key ?? json.RootElement.GetProperty("id").GetString()!;
            writer.WriteString("id", cosmos ? CosmosId(identity) : identity);
            if (cosmos) writer.WriteString("key", identity);
            foreach (var property in json.RootElement.EnumerateObject())
                if (property.Name is not ("id" or "key" or "@metadata" or "_id") && !property.Name.StartsWith('_')) property.WriteTo(writer);
            if (ravenCollection != null)
            { writer.WriteStartObject("@metadata"); writer.WriteString("@collection", ravenCollection); writer.WriteEndObject(); }
            writer.WriteEndObject();
        }
        return buffer.WrittenSpan.ToArray();
    }
    internal static BsonDocument Mongo(ReadOnlyMemory<byte> bytes)
    {
        using var json = JsonDocument.Parse(bytes);
        var doc = (BsonDocument)Bson(json.RootElement);
        doc["_id"] = doc["id"];
        return doc;
    }
    private static BsonValue Bson(JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                var document = new BsonDocument();
                foreach (var property in value.EnumerateObject()) document.Add(property.Name, Bson(property.Value));
                return document;
            case JsonValueKind.Array:
                var array = new BsonArray(); foreach (var item in value.EnumerateArray()) array.Add(Bson(item)); return array;
            case JsonValueKind.String: return new BsonString(value.GetString());
            case JsonValueKind.Number: return value.TryGetInt64(out long integer) ? new BsonInt64(integer) : new BsonDecimal128(value.GetDecimal());
            case JsonValueKind.True: return BsonBoolean.True;
            case JsonValueKind.False: return BsonBoolean.False;
            default: return BsonNull.Value;
        }
    }
}
