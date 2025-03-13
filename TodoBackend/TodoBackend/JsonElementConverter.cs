using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TodoBackend;

/// <summary>
///  <see cref="JsonElement"/>
/// </summary>
public class JsonElementConverter : ValueConverter<JsonElement, string>
{
    /// <summary>
    ///  Initializes a new instance of the <see cref="JsonElementConverter"/> class.
    /// </summary>
    public JsonElementConverter()
        : base(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = false }),
            v => JsonDocument.Parse(v, default).RootElement.Clone())
    {
    }
}
