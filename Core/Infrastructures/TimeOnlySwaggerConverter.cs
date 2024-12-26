using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Infrastructures;

public class TimeOnlySwaggerConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        int hour = jsonObject.GetProperty("hour").GetInt32();
        int minute = jsonObject.GetProperty("minute").GetInt32();
        return new TimeOnly(hour, minute);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("hour", value.Hour);
        writer.WriteNumber("minute", value.Minute);
        writer.WriteEndObject();
    }
}
