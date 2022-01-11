using SSDLMaintenanceTool.Events;

namespace SSDLMaintenanceTool.Interfaces
{
    public interface IWorkflowEventJsonValidator
    {
        BaseJsonEvent GetJsonEvent(string queryName, string query, int sequenceId);
    }
}
