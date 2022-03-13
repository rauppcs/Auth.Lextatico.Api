using Auth.Lextatico.Application.Services;
using Auth.Lextatico.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureApplicationServices
    {
        public static IServiceCollection AddLextaticoApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthAppService, AuthAppService>();

            return services;
        }
    }
}
