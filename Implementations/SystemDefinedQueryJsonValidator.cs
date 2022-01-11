using SSDLMaintenanceTool.Events;
using SSDLMaintenanceTool.Interfaces;

namespace SSDLMaintenanceTool.Implementations
{
    public class SystemDefinedQueryJsonValidator : IWorkflowEventJsonValidator
    {
        public BaseJsonEvent GetJsonEvent(string queryName, string query, int sequenceId)
        {
            var jsonEvent = new SystemDefinedQueryJsonEvent();
            jsonEvent.QueryName = queryName;
            jsonEvent.SystemDefinedQuery = query.Replace("'", "''");
            jsonEvent.QuerySequence = sequenceId.ToString();
            return jsonEvent;
        }
    }
}
