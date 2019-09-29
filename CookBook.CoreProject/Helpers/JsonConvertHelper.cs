using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CookBook.CoreProject.Helpers
{
    public static class JsonConvertHelper
    {
        public static string SerializeObjectCamelCase(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
