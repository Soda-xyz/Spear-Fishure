namespace SpearFishure.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Model for fissure data.
    /// </summary>
    public class FissureModel
    {
        required public string Id { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(ExpiryDateTimeConverter))]
        public DateTime Expiry { get; set; }

        required public string Node { get; set; }

        public bool? Hard { get; set; }

        /// <summary>
        /// Json Deserializer for WF API.
        /// </summary>
        /// <param name="json">The JSON string representing a list of fissures.</param>
        /// <returns>A list of <see cref="FissureModel"/> objects, or an empty list if deserialization fails.</returns>
        public static List<FissureModel> FromJson(string json)
        {
            return JsonSerializer.Deserialize<List<FissureModel>>(json) ?? new List<FissureModel>();
        }
    }

    public class ExpiryDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        /// <inheritdoc/>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (var doc = JsonDocument.ParseValue(ref reader))
                {
                    if (doc.RootElement.TryGetProperty("$date", out var dateElem) &&
                        dateElem.TryGetProperty("$numberLong", out var numLongElem) &&
                        long.TryParse(numLongElem.GetString(), out var ms))
                    {
                        // Convert from Unix ms to DateTime (UTC)
                        return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
                    }
                }
            }

            throw new JsonException("Invalid Expiry format");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Write as ISO8601 string for simplicity
            writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
        }
    }
}
