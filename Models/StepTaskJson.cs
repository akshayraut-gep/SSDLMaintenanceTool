using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SSDLMaintenanceTool
{
    public class StepTaskJson
    {
        [JsonPropertyName("isSelected")]
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }

        [JsonPropertyName("isExpanded")]
        [JsonProperty("isExpanded")]
        public bool IsExpanded { get; set; }

        [JsonPropertyName("sequenceId")]
        [JsonProperty("sequenceId")]
        public int SequenceId { get; set; }

        [JsonPropertyName("childEventUIDetail")]
        [JsonProperty("childEventUIDetail")]
        public List<ChildEventUIDetail> ChildEventUIDetail { get; set; }

        [JsonPropertyName("eventUIDetail")]
        [JsonProperty("eventUIDetail")]
        public List<object> EventUIDetail { get; set; }

        public StepTaskJson()
        {
            EventUIDetail = new List<object>();
            ChildEventUIDetail = new List<ChildEventUIDetail>();
        }
    }
}
