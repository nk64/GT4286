using System.Text.Json;
using System.Text.Json.Serialization;
using GT4286Util.Entities;

namespace GT4286Util.Helpers
{
    [JsonSerializable(typeof(Game))]
    [JsonSerializable(typeof(IEnumerable<Game>))]
    [JsonSerializable(typeof(GenerationInfo))]
    [JsonSerializable(typeof(IEnumerable<GenerationInfo>))]
    public partial class MyContext : JsonSerializerContext { }

    public static class SerializationHelper
    {

        
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(){
            WriteIndented = true,
            AllowTrailingCommas = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            TypeInfoResolver = MyContext.Default,
        };

        public static string SerializeToJsonString<T>(T o)
        {
            return JsonSerializer.Serialize<T>(o, jsonSerializerOptions);
        }

        public static T DeserializeJsonString<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString, jsonSerializerOptions)!;
        }

    }
}