using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Incyte.Resource
{
    public class DBAccess : IDisposable
    {
        
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataReader datareader = null;


        public SqlDataReader ExecuteQuery(string sql, string sConn)
        {
            connection = new SqlConnection(sConn);
            command = new SqlCommand(sql, connection);
            connection.Open();
            return command.ExecuteReader();
        }

        public SqlDataReader ExecuteProcedure(string name, string sConn, SqlParameter[] parameters)
        {
            connection = new SqlConnection(sConn);
            command = new SqlCommand(name, connection);
            connection.Open();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            return command.ExecuteReader();
        }

        public void ExecuteScalar(string sql, string sConn)
        {
            connection = new SqlConnection(sConn);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteScalar();
        }
        
        public void Dispose()
        {
            if (datareader != null && !datareader.IsClosed)
            {
                datareader.Close();
                datareader.Dispose();
            }

            if (command != null)
                command.Dispose();

            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
                connection.Dispose();
            }
            
            
        }
    }
}
