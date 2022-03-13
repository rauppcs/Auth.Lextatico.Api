using Auth.Lextatico.Infra.Services.Implementations;
using Auth.Lextatico.Infra.Services.Interfaces;
using Auth.Lextatico.Infra.Services.Models.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureInfraServices
    {
        public static IServiceCollection AddLextaticoEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var password = configuration["EmailPassword"];

            services.Configure<EmailSettings>(options =>
            {
                options.Email = emailSettings.Email;
                options.DisplayName = emailSettings.DisplayName;
                options.Host = emailSettings.Host;
                options.Password = password;
                options.Port = emailSettings.Port;
            });

            return services;
        }
        public static IServiceCollection AddLextaticoInfraServices(this IServiceCollection services)
        {
            // INFRA SERVICES
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
