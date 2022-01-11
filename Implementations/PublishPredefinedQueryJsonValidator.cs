using SSDLMaintenanceTool.Events;
using SSDLMaintenanceTool.Interfaces;

namespace SSDLMaintenanceTool.Implementations
{
    public class PublishPredefinedQueryJsonValidator : IWorkflowEventJsonValidator
    {
        public BaseJsonEvent GetJsonEvent(string queryName, string query, int sequenceId)
        {
            var jsonEvent = new PublishPredefinedQuery();
            jsonEvent.QueryName = queryName;
            jsonEvent.JsonFormat = query.Replace("'", "''");
            jsonEvent.QueryId = sequenceId.ToString();
            jsonEvent.QuerySequence = sequenceId.ToString();
            jsonEvent.ConfiguredSetting = "PreDefinedSteps_Configured";
            return jsonEvent;
        }
    }
}
