using Newtonsoft.Json;
using System.Text.Json.Serialization;
using CC = SSDLMaintenanceTool.Constants.QueryConstants;

namespace SSDLMaintenanceTool.Events
{
    public class SystemDefinedQueryJsonEvent : BaseQueryJsonEvent
    {
        [JsonProperty(CC.SystemDefinedQuery)]
        [JsonPropertyName(CC.SystemDefinedQuery)]
        public string SystemDefinedQuery { get; set; }
    }
}
