using System.IO;
using Newtonsoft.Json;


namespace Mimeo.Blocks.Serialization
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();

        public static string SerializeToJson(this object input)
        {
            var stringWriter = new StringWriter();
            input.SerializeToJson(stringWriter);
            return stringWriter.ToString();
        }

        public static void SerializeToJson(this object input, TextWriter textWriter)
        {
            var writer = new JsonTextWriter(textWriter);
            var serializer = JsonSerializer.Create(SerializerSettings);
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, input);
            writer.Flush();
        }

        public static T DeserializeFromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
 