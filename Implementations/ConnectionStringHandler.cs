using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SSDLMaintenanceTool.Implementations
{
    public class ConnectionStringHandler
    {
        public static List<ConnectionDetails> ConnectionStrings { get; set; }

        static ConnectionStringHandler()
        {
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), @"Assets\connection-strings.json");
            var json = File.ReadAllText(jsonFilePath);
            if (json.HasContent())
            {
                ConnectionStrings = JsonSerializer.Deserialize<List<ConnectionDetails>>(json);
            }
        }

        public static ConnectionDetails GetDeepCopy(ConnectionDetails sourceConnectionDetails)
        {
            var newConnectionDetails = new ConnectionDetails();
            newConnectionDetails.ConfigDBConnectionName = sourceConnectionDetails.ConfigDBConnectionName;
            newConnectionDetails.Database = sourceConnectionDetails.Database;
            newConnectionDetails.DisplayName = sourceConnectionDetails.DisplayName;
            newConnectionDetails.IsInputCredentialsRequired = sourceConnectionDetails.IsInputCredentialsRequired;
            newConnectionDetails.Name = sourceConnectionDetails.Name;
            newConnectionDetails.Password = sourceConnectionDetails.Password;
            newConnectionDetails.Server = sourceConnectionDetails.Server;
            newConnectionDetails.UserName = sourceConnectionDetails.UserName;
            newConnectionDetails.IsMFA = sourceConnectionDetails.IsMFA;
            return newConnectionDetails;
        }
    }
}
