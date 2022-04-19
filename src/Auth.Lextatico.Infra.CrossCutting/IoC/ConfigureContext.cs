using Auth.Lextatico.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureContext
    {
        public static IServiceCollection AddLextaticoContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            services.AddDbContext<LextaticoContext>(op =>
            {
                var connectionString = configuration.GetConnectionString(nameof(LextaticoContext));

                if (!hostEnvironment.IsProduction())
                {
                    var sqlStringBuilder = new SqlConnectionStringBuilder(connectionString);

                    sqlStringBuilder.Password = configuration["DbPassword"];

                    connectionString = sqlStringBuilder.ToString();
                }

                op.UseLazyLoadingProxies();

                op.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
