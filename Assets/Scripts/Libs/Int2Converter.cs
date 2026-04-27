using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

public class Int2Converter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(int2) || objectType == typeof(int2?);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }

        int2 v = (int2)value;

        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            if (objectType == typeof(int2?))
                return null;

            return new int2();
        }

        var obj = JObject.Load(reader);

        var result = new int2(
            (int)obj["x"],
            (int)obj["y"]
        );

        if (objectType == typeof(int2?))
            return (int2?)result;

        return result;
    }
}