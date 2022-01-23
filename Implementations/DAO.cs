using SSDLMaintenanceTool.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SSDLMaintenanceTool.Implementations
{
    public class DAO
    {
        private string GetConnectionString(ConnectionDetails connectionDetails)
        {
            return "Server=" + connectionDetails.Server + ";Database=" + connectionDetails.Database + ";User Id=" + connectionDetails.UserName + (connectionDetails.IsMFA ? "" : ";Password=" + connectionDetails.Password) + "; MultipleActiveResultSets = True; ConnectRetryCount = 3; ConnectRetryInterval = 10; Connection Timeout = 30; Trusted_Connection = False; Encrypt = True;";
        }

        public DataSet GetData(string query, ConnectionDetails connectionDetails)
        {
            var connectionString = GetConnectionString(connectionDetails);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connectionString);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }
    }
}
