using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions
{
    public static class SerilogExtensions
    {
        public static void UseLextaticoSerilog(this IHostBuilder builder, IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.UseSerilog();
        }
    }
}