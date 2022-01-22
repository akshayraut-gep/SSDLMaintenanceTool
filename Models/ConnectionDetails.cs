namespace SSDLMaintenanceTool.Models
{
    public class ConnectionDetails
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsInputCredentialsRequired { get; set; }
        public string ConfigDBConnectionName { get; set; }
        public bool IsMFA { get; set; }
    }
}
