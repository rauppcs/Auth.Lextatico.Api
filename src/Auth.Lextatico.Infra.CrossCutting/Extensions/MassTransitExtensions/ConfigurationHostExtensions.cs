using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class ConfigurationHostExtensions
    {
        public static void ConfigurationRabbitMqAccountHost(this IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration.GetConnectionString("RabbitMqAccount"), config =>
            {
                config.PublisherConfirmation = true;
            });
        }

        public static void ConfigurationServiceBusAccountHost(this IServiceBusBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration.GetConnectionString("ServiceBusAccount"), config =>
            {
            });
        }
    }
}
