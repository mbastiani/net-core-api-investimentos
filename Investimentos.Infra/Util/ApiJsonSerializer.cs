using System.Text.Json;
using System.Text.Json.Serialization;

namespace Investimentos.Infra.Util
{
    public class ApiJsonSerializer
    {
        private readonly JsonSerializerOptions _Options;

        public ApiJsonSerializer()
        {
            _Options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _Options);
        }

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _Options);
        }
    }
}