using Auth.Lextatico.Application.Consumers.ApplicationUser;
using Auth.Lextatico.Application.ConsumersDefinition.ApplicationUser;
using Auth.Lextatico.Infra.Services.MessageBroker.Bus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddLextaticoMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMqAccountHost(configuration);

            return services;
        }

        private static IServiceCollection AddRabbitMqAccountHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<IAccountBus>(x =>
            {
                x.AddConsumer<UserCreatedEventConsumer>(typeof(UserCreatedEventConsumerDefinition));

                x.AddConsumer<UserUpdatedEventConsumer>(typeof(UserUpdatedEventConsumerDefinition));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigurationRabbitMqAccountHost(configuration);

                    cfg.UseRawJsonSerializer(isDefault: true);

                    cfg.UseRawJsonDeserializer(isDefault: true);

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        public static IServiceCollection AddLextaticoMassTransitWithServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceBusAccountHost(configuration);

            return services;
        }

        private static IServiceCollection AddServiceBusAccountHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<IAccountBus>(x =>
            {
                x.AddConsumer<UserCreatedEventConsumer>(typeof(UserCreatedEventConsumerDefinition));

                x.AddConsumer<UserUpdatedEventConsumer>(typeof(UserUpdatedEventConsumerDefinition));

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.ConfigurationServiceBusAccountHost(configuration);

                    cfg.UseRawJsonSerializer(isDefault: true);

                    cfg.UseRawJsonDeserializer(isDefault: true);

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
