using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace InstaBotPrototype.Extensions
{
    public static class ConnectionExtensions
    {
        public static IWebHost CheckConnection(this IWebHost webHost) //test connection to database
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                ILogger<object> logger = services.GetRequiredService<ILogger<Object>>(); //getting logger


                var configuration = services.GetService<IConfiguration>(); //getting connectionString from appsettings.json
                var connectionString = configuration.GetConnectionString("connectionString"); 


                using (SqlConnection conn = new SqlConnection(connectionString))  //creating connection
                {
                    try //проверить возможность подключения
                    {
                        conn.Open();//вывести сообщение об успешном в логгер

                        conn.Close();
                        logger.LogInformation("Sucsessfully tested connection with connectionString: \"{ConnectionString}\"", conn);
                        //LOG SUCSESS
                    }
                    catch (SqlException exception) //вывести сообщение об неуспешном подключении в логгер, с ошибкой
                    {
                        logger.LogInformation("Connection to database failed with error: \"{Message}\"", exception);
                        //LOG FAIL OF CONNECT TO DB
                    }
                }

            }
            return webHost;
        }
    }
}