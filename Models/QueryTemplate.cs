namespace SSDLMaintenanceTool.Models
{
    public class QueryTemplate : NameValueModel
    {
        public string QueryTemplateFilePath { get; set; }
        public QueryCompletionCallback QueryCompletionCallback { get; set; }
    }

    public class QueryCompletionCallback
    {
        public string Assembly { get; set; }
        public string ClassName { get; set; }
        public string Method { get; set; }
    }
}
