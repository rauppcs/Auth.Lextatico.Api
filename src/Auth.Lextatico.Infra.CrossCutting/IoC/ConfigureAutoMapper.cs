using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureAutoMapper
    {
        public static IServiceCollection AddLextaticoAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.Load("Auth.Lextatico.Application"));

            return services;
        }
    }
}
