using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShareCommon.Helper
{
    public class RawJsonConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var raw = JsonDocument.ParseValue(ref reader);
            return raw.RootElement.GetRawText();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            using var @object = JsonDocument.Parse(value);
            @object.RootElement.WriteTo(writer);
        }
    }
}
