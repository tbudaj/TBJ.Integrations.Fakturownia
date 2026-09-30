using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Internal;

/// <summary>
/// Konwerter <see cref="decimal"/> tolerujący formaty zwracane przez Fakturownia API:
/// liczba JSON, string "12.50" lub "12,50", pusty string oraz null.
/// </summary>
internal sealed class FlexibleDecimalConverter : JsonConverter<decimal?>
{
    /// <summary>Odczytuje wartość dziesiętną z tokenu JSON (null, liczba lub string).</summary>
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.Number:
                return reader.GetDecimal();
            case JsonTokenType.String:
                var text = reader.GetString();
                if (string.IsNullOrWhiteSpace(text))
                    return null;

                return decimal.TryParse(
                    text.Replace(',', '.'),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var value)
                    ? value
                    : null;
            default:
                throw new JsonException($"Nieobsługiwany token JSON: {reader.TokenType}.");
        }
    }

    /// <summary>Zapisuje wartość dziesiętną jako liczbę JSON lub null.</summary>
    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            writer.WriteNumberValue(value.Value);
    }
}
