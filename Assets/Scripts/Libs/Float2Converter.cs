using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

public class Float2Converter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(float2) || objectType == typeof(float2?);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }

        float2 v = (float2)value;

        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            if (objectType == typeof(float2?))
                return null;

            return new float2();
        }

        var obj = JObject.Load(reader);
        var result = new float2(
            (float)obj["x"],
            (float)obj["y"]
        );

        if (objectType == typeof(float2?))
            return (float2?)result;

        return result;
    }
}