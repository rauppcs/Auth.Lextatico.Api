using Auth.Lextatico.Infra.CrossCutting.CustomChecks;
using Auth.Lextatico.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureHealthChecks
    {
        public static IServiceCollection AddLextaticoHealthChecks(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            var connectionString = configuration.GetConnectionString(nameof(LextaticoContext));

            if (!hostEnvironment.IsProduction())
            {
                var sqlStringBuilder = new SqlConnectionStringBuilder(connectionString);

                sqlStringBuilder.Password = configuration["DbPassword"];

                connectionString = sqlStringBuilder.ToString();
            }

            services.AddHealthChecks()
                .AddCheck<SelfCheck>("api")
                .AddMongoDb(configuration.GetConnectionString("LextaticoMongoDbLogs"), name: "mongodb")
                .AddSqlServer(connectionString, name: "sqlserver");

            return services;
        }
    }
}
