﻿namespace SSDLMaintenanceTool.Models
{
    public class QueryTemplate : NameValueModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string QueryTemplateFilePath { get; set; }
    }
}
