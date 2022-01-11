using Newtonsoft.Json;
using System.Text.Json.Serialization;
using CC = SSDLMaintenanceTool.Constants.QueryConstants;

namespace SSDLMaintenanceTool.Events
{
    public class CleansingAtSourceJsonEvent : BaseJsonEvent
    {
        [JsonProperty(CC.QueryName)]
        [JsonPropertyName(CC.QueryName)]
        public string QueryName { get; set; }

        [JsonProperty(CC.QueryId)]
        [JsonPropertyName(CC.QueryId)]
        public string QueryId { get; set; }

        [JsonProperty(CC.Query)]
        [JsonPropertyName(CC.Query)]
        public string Query { get; set; }

        [JsonProperty(CC.SortOrder)]
        [JsonPropertyName(CC.SortOrder)]
        public string SortOrder { get; set; }
    }
}
