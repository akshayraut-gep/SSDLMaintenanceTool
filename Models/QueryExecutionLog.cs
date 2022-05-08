namespace SSDLMaintenanceTool.Models
{
    public class QueryExecutionLog
    {
        public string Query { get; set; }
        public string DatabaseName { get; set; }
        public string ErrorMessage { get; set; }
        public long RowsAffected { get; set; }
    }
}
