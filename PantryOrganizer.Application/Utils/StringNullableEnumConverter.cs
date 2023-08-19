using System.Text.Json;
using System.Text.Json.Serialization;

namespace PantryOrganizer.Application.Utils;

public class StringNullableEnumConverter<TEnum> : JsonConverter<TEnum>
{
    private readonly JsonConverter<TEnum>? converter;
    private readonly Type? underlyingType;

    public StringNullableEnumConverter() : this(null) { }

    public StringNullableEnumConverter(JsonSerializerOptions? options)
    {
        if (options != null)
            converter = (JsonConverter<TEnum>)options.GetConverter(typeof(TEnum));

        underlyingType = Nullable.GetUnderlyingType(typeof(TEnum));
    }

    public override bool CanConvert(Type typeToConvert)
        => typeof(TEnum).IsAssignableFrom(typeToConvert);

    public override TEnum? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (underlyingType == null)
            return default;

        if (converter != null)
            return converter.Read(ref reader, underlyingType, options);

        string? value = reader.GetString();

        return string.IsNullOrEmpty(value)
            ? default
            : !Enum.TryParse(underlyingType, value, false, out object? result)
            && !Enum.TryParse(underlyingType, value, true, out result)
            ? throw new JsonException(
                $"Unable to convert \"{value}\" to Enum \"{underlyingType}\".")
            : (TEnum)result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TEnum value,
        JsonSerializerOptions options)
        => writer.WriteStringValue(value?.ToString());
}
