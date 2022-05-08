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
            if (connectionDetails.IsMFA)
                return "Driver={ODBC Driver 17 for SQL Server};SERVER=" + connectionDetails.Server + "; DATABASE=" + connectionDetails.Database + ";Authentication=ActiveDirectoryInteractive;UID=";
            else
                return "Server=" + connectionDetails.Server + ";Database=" + connectionDetails.Database + ";User Id=" + connectionDetails.UserName + (connectionDetails.IsMFA ? "" : ";Password=" + connectionDetails.Password) + "; MultipleActiveResultSets = True; ConnectRetryCount = 3; ConnectRetryInterval = 10; Connection Timeout = 30; Trusted_Connection = False;" + (connectionDetails.Environment == "Local" ? "" : "Encrypt = True;");
        }

        private string GetConnectionString(ConnectionStringsByRegion connectionStringByRegion)
        {
            if (connectionStringByRegion.IsMFA)
                return "Driver={ODBC Driver 17 for SQL Server};SERVER=" + connectionStringByRegion.Server + "; DATABASE=" + connectionStringByRegion.Database + ";Authentication=ActiveDirectoryInteractive;UID=";
            else
                return "Server=" + connectionStringByRegion.Server + ";Database=" + connectionStringByRegion.Database + ";User Id=" + connectionStringByRegion.UserName + (connectionStringByRegion.IsMFA ? "" : ";Password=" + connectionStringByRegion.Password) + "; MultipleActiveResultSets = True; ConnectRetryCount = 3; ConnectRetryInterval = 10; Connection Timeout = 30; Trusted_Connection = False;" + (connectionStringByRegion.Environment == "Local" ? "" : "Encrypt = True;");
        }

        private IDbDataAdapter GetAdapter(string query, ConnectionDetails connectionDetails)
        {
            var connectionString = GetConnectionString(connectionDetails);
            if (connectionDetails.IsMFA)
                return new OdbcDataAdapter(query, connectionString);
            else
                return new SqlDataAdapter(query, connectionString);
        }

        private IDbDataAdapter GetAdapter(string query, ConnectionStringsByRegion connectionStringByRegion)
        {
            var connectionString = GetConnectionString(connectionStringByRegion);
            if (connectionStringByRegion.IsMFA)
                return new OdbcDataAdapter(query, connectionString);
            else
                return new SqlDataAdapter(query, connectionString);
        }

        private IDbConnection GetConnectionWithCommand(string query, ConnectionDetails connectionDetails, out IDbCommand dbCommand)
        {
            var connectionString = GetConnectionString(connectionDetails);
            IDbConnection dbConnection;
            if (connectionDetails.IsMFA)
            {
                dbConnection = new OdbcConnection(connectionString);
                dbCommand = new OdbcCommand(query, dbConnection as OdbcConnection);
            }
            else
            {
                dbConnection = new SqlConnection(connectionString);
                dbCommand = new SqlCommand(query, dbConnection as SqlConnection);
            }
            return dbConnection;
        }

        public DataSet GetData(string query, ConnectionDetails connectionDetails)
        {
            DataSet dataSet = new DataSet();

            var dbAdapter = GetAdapter(query, connectionDetails);
            dbAdapter.SelectCommand.CommandTimeout = 0;
            dbAdapter.Fill(dataSet);

            return dataSet;
        }

        public DataSet GetData(string query, ConnectionStringsByRegion connectionStringByRegion)
        {
            DataSet dataSet = new DataSet();

            var dbAdapter = GetAdapter(query, connectionStringByRegion);
            dbAdapter.SelectCommand.CommandTimeout = 0;
            dbAdapter.Fill(dataSet);

            return dataSet;
        }

        public int ChangeData(string query, ConnectionDetails connectionDetails)
        {
            IDbCommand dbCommand;
            using (var dbConnection = GetConnectionWithCommand(query, connectionDetails, out dbCommand))
            {
                dbCommand.CommandTimeout = 0;
                dbConnection.Open();
                using (dbCommand)
                {
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
