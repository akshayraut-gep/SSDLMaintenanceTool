using SSDLMaintenanceTool.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SSDLMaintenanceTool.Implementations
{
    public class DAO
    {
        public DataSet GetData(string query, ConnectionDetails connectionDetails)
        {
            var connectionString = "Server=" + connectionDetails.Server + ";Database=" + connectionDetails.Database + ";User Id=" + connectionDetails.UserName + (connectionDetails.IsMFA ? "" : ";Password=" + connectionDetails.Password) + "; MultipleActiveResultSets = True; ConnectRetryCount = 3; ConnectRetryInterval = 10; Connection Timeout = 30; Trusted_Connection = False; Encrypt = True;";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connectionString);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }
    }
}
