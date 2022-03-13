using Auth.Lextatico.Domain.Dtos.Message;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureMessage
    {
        public static IServiceCollection AddLextaticoMessage(this IServiceCollection services)
        {
            services.AddScoped<IMessage, Message>();

            return services;
        }
    }
}
