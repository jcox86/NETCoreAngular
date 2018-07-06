using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NETCore_Angular1.Models
{
    public sealed class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
        }
    }
}
