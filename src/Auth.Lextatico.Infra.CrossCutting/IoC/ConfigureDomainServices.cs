using Auth.Lextatico.Domain.Interfaces.Services;
using Auth.Lextatico.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureDomainServices
    {
        public static IServiceCollection AddLextaticoDomainServices(this IServiceCollection services)
        {
            // DOMAIN SERVICES
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
