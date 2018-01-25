using System;
using System.Collections.Generic;
using System.Linq;
using InstaBotLibrary.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace InstaBotLibrary.Repository
{
    public abstract class Repository
    {
        string connectionString;

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public Repository()
        {
            connectionString = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build()
               .GetConnectionString("connectionString");
        }



        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }


       
    }
}
