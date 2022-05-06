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

        public ConnectionStringHandler(string connectionStringPath)
        {
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), connectionStringPath);
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

            if (source.ConnectionStringByRegions != null && source.ConnectionStringByRegions.Count > 0)
            {
                foreach (var connectionStringByRegionIterator in source.ConnectionStringByRegions)
                {
                    var copyConnectionStringByRegion = new ConnectionStringsByRegion();
                    copyConnectionStringByRegion.Name = connectionStringByRegionIterator.Name;
                    copyConnectionStringByRegion.Server = connectionStringByRegionIterator.Server;
                    copyConnectionStringByRegion.Database = connectionStringByRegionIterator.Database;
                    copyConnectionStringByRegion.IsInputCredentialsRequired = connectionStringByRegionIterator.IsInputCredentialsRequired;
                    copyConnectionStringByRegion.IsMFA = connectionStringByRegionIterator.IsMFA;
                    copyConnectionStringByRegion.UserName = connectionStringByRegionIterator.UserName;
                    copyConnectionStringByRegion.Password = connectionStringByRegionIterator.Password;
                    copyConnectionStringByRegion.ConfigDBConnectionName = connectionStringByRegionIterator.ConfigDBConnectionName;
                    copyConnectionStringByRegion.IsMultiTenant = connectionStringByRegionIterator.IsMultiTenant;
                    copyConnectionStringByRegion.Environment = connectionStringByRegionIterator.Environment;
                    copyConnectionStringByRegion.Region = connectionStringByRegionIterator.Region;
                    copyConnectionStringByRegion.Instance = connectionStringByRegionIterator.Instance;
                    copy.ConnectionStringByRegions.Add(copyConnectionStringByRegion);
                }
            }
            return copy;
        }
    }
}
