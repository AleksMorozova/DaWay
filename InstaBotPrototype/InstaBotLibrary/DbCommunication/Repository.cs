using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace InstaBotLibrary.DbCommunication
{
    public abstract class Repository
    {
        //string connectionString;
        private IDbConnectionFactory factory;

        public Repository(IDbConnectionFactory connectionFactory)
        {
            factory = connectionFactory;
        }

        protected IDbConnection GetConnection()
        {
            return factory.GetConnection();
            //return new SqlConnection(connectionString);
        }
    }
}
