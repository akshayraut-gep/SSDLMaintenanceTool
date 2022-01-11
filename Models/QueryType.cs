using System;

namespace SSDLMaintenanceTool
{
    internal class QueryType
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public int StageId { get; set; }
        public bool AllowMultiple { get; set; }
        public string DisplayName { get; set; }
        public int ActivityId { get; set; }
        public Type QueryTypeValidator { get; set; }

        public QueryType(int activityId, int stageId, int eventId, string name, Type queryTypeValidator, bool allowMultiple = false)
        {
            this.ActivityId = activityId;
            this.EventId = eventId;
            this.Name = name;
            this.StageId = stageId;
            this.AllowMultiple = allowMultiple;
            this.DisplayName = name + " (" + eventId.ToString() + ")";
            this.QueryTypeValidator = queryTypeValidator;
        }
    }
}