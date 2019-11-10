using System.Text.Json;

namespace CookBook.CoreProject.Helpers
{
    public static class JsonConvertHelper
    {
        public static string SerializeObjectCamelCase(object value)
        {
            return JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
