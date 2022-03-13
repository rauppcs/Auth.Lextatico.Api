using Auth.Lextatico.Infra.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureHealthChecks
    {
        public static IServiceCollection AddLextaticoHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString(nameof(LextaticoContext)));

            sqlStringBuilder.Password = configuration["DbPassword"];

            var connectionString = sqlStringBuilder.ToString();

            services.AddHealthChecks()
                .AddSqlServer(connectionString, name: "SqlServer");

            services.AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;
        }
    }
}
