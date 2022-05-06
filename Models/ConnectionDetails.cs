using System.Collections.Generic;

namespace SSDLMaintenanceTool.Models
{
    public class ConnectionDetails
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public bool IsInputCredentialsRequired { get; set; }
        public bool IsMFA { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfigDBConnectionName { get; set; }
        public bool IsMultiTenant { get; set; }
        public string Environment { get; set; }
        public string Region { get; set; }
        public string Instance { get; set; }
        public List<ConnectionStringsByRegion> ConnectionStringByRegions { get; set; }

        public ConnectionDetails()
        {
            ConnectionStringByRegions = new List<ConnectionStringsByRegion>();
        }
    }

    public class ConnectionStringsByRegion
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public bool IsInputCredentialsRequired { get; set; }
        public bool IsMFA { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfigDBConnectionName { get; set; }
        public bool IsMultiTenant { get; set; }
        public string Environment { get; set; }
        public string Region { get; set; }
        public string Instance { get; set; }
    }
}
