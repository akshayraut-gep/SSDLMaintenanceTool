using SSDLMaintenanceTool.Events;
using SSDLMaintenanceTool.Interfaces;

namespace SSDLMaintenanceTool.Implementations
{
    public class CleansingAtSourceJsonValidator : IWorkflowEventJsonValidator
    {
        public BaseJsonEvent GetJsonEvent(string queryName, string query, int sequenceId)
        {
            var jsonEvent = new CleansingAtSourceJsonEvent();
            jsonEvent.QueryName = queryName;
            jsonEvent.Query = query.Replace("'", "''");
            jsonEvent.SortOrder = sequenceId.ToString();
            return jsonEvent;
        }
    }
}
