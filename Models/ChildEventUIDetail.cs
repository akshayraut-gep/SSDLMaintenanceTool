using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SSDLMaintenanceTool
{
    public class ChildEventUIDetail
    {
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("isSelected")]
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }

        [JsonPropertyName("sequenceId")]
        [JsonProperty("sequenceId")]
        public int SequenceId { get; set; }

        [JsonPropertyName("eventId")]
        [JsonProperty("eventId")]
        public int EventId { get; set; }
    }
}
