using Newtonsoft.Json;
using System.Text.Json.Serialization;
using CC = SSDLMaintenanceTool.Constants.QueryConstants;

namespace SSDLMaintenanceTool.Events
{
    public class PublishPredefinedQuery : BaseJsonEvent
    {
        [JsonProperty(CC.JsonFormat)]
        [JsonPropertyName(CC.JsonFormat)]
        public string JsonFormat { get; set; }

        [JsonProperty(CC.QueryName)]
        [JsonPropertyName(CC.QueryName)]
        public string QueryName { get; set; }

        [JsonProperty(CC.QuerySequence)]
        [JsonPropertyName(CC.QuerySequence)]
        public string QuerySequence { get; set; }

        [JsonProperty(CC.QueryId)]
        [JsonPropertyName(CC.QueryId)]
        public string QueryId { get; set; }

        [JsonProperty(CC.ConfiguredSetting)]
        [JsonPropertyName(CC.ConfiguredSetting)]
        public string ConfiguredSetting { get; set; }
    }
}
