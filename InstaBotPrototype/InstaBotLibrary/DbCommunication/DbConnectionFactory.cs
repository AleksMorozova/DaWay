using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Options;

namespace InstaBotLibrary.DbCommunication
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private string connectionString;
        public DbConnectionFactory(IOptions<DbConnectionOptions> options)
        {
            connectionString = options.Value.connectionString;
        }
        

        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
