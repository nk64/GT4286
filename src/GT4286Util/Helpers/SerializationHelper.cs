using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GT4286Util.Entities;

namespace GT4286Util.Helpers
{
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        AllowTrailingCommas = true
        //Converters = [typeof(JsonStringEnumConverter<GenerationId>)]
    )]
    [JsonSerializable(typeof(Game))]
    [JsonSerializable(typeof(IEnumerable<Game>))]
    [JsonSerializable(typeof(GenerationInfo))]
    [JsonSerializable(typeof(IEnumerable<GenerationInfo>))]
    [JsonSerializable(typeof(SimplePatchData))]
    public partial class MyContext : JsonSerializerContext { }

    public static class SerializationHelper
    {
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(){
            WriteIndented = true,
            AllowTrailingCommas = true,
            Converters =
            {
                new JsonStringEnumConverter<GenerationId>(JsonNamingPolicy.CamelCase)
            },
            TypeInfoResolver = MyContext.Default,
        };

        public static readonly MyContext JsonSerializerContext = new MyContext(jsonSerializerOptions);

        public static string SerializeToJsonString<T>(T o, JsonTypeInfo<T> jsonTypeInfo)
        {
            return JsonSerializer.Serialize<T>(o, jsonTypeInfo);
        }

        public static T DeserializeJsonString<T>(string jsonString, JsonTypeInfo<T> jsonTypeInfo)
        {
            return JsonSerializer.Deserialize<T>(jsonString, jsonTypeInfo)!;
        }

    }
}