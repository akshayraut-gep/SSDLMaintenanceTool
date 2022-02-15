using SSDLMaintenanceTool.Models;
using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace SSDLMaintenanceTool.Implementations
{
    public class DAO
    {
        private string GetConnectionString(ConnectionDetails connectionDetails)
        {
            return "Server=" + connectionDetails.Server + ";Database=" + connectionDetails.Database + ";User Id=" + connectionDetails.UserName + (connectionDetails.IsMFA ? "" : ";Password=" + connectionDetails.Password) + "; MultipleActiveResultSets = True; ConnectRetryCount = 3; ConnectRetryInterval = 10; Connection Timeout = 30; Trusted_Connection = False;" + (connectionDetails.Environment == "Local" ? "" : "Encrypt = True;");
        }

        public DataSet GetData(string query, ConnectionDetails connectionDetails)
        {
            if (connectionDetails.IsMFA)
                return GetDataWithMFA(query, connectionDetails);
            else
                return GetDataWithoutMFA(query, connectionDetails);
        }

        public DataSet GetDataWithoutMFA(string query, ConnectionDetails connectionDetails)
        {
            DataSet dataSet = new DataSet();

            var connectionString = GetConnectionString(connectionDetails);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connectionString);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.Fill(dataSet);

            return dataSet;
        }

        public DataSet GetDataWithMFA(string query, ConnectionDetails connectionDetails)
        {
            DataSet dataSet = new DataSet();

            OdbcConnection con = new OdbcConnection("Driver={ODBC Driver 17 for SQL Server};SERVER=" + connectionDetails.Server + "; DATABASE=" + connectionDetails.Database + ";Authentication=ActiveDirectoryInteractive;UID=");
            OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, con);
            odbcDataAdapter.SelectCommand.CommandTimeout = 0;
            odbcDataAdapter.Fill(dataSet);

            return dataSet;
        }
    }
}
