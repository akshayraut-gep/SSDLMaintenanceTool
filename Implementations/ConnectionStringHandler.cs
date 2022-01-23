using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

        public ConnectionDetails GetDeepCopy(ConnectionDetails source)
        {
            var copy = new ConnectionDetails();
            copy.DisplayName = source.DisplayName;
            copy.Name = source.Name;
            copy.Server = source.Server;
            copy.Database = source.Database;
            copy.IsInputCredentialsRequired = source.IsInputCredentialsRequired;
            copy.IsMFA = source.IsMFA;
            copy.UserName = source.UserName;
            copy.Password = source.Password;
            copy.ConfigDBConnectionName = source.ConfigDBConnectionName;
            copy.IsMultiTenant = source.IsMultiTenant;
            copy.Environment = source.Environment;
            copy.Region = source.Region;
            copy.Instance = source.Instance;
            return copy;
        }
    }
}
