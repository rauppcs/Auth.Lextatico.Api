using MassTransit;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class AddPublisherExtensions
    {
        public static IRabbitMqBusFactoryConfigurator AddMassTransitDirectPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddMassTransitPublisher<T>(exchangeName, ExchangeType.Direct);

            return cfg;
        }

        public static IRabbitMqBusFactoryConfigurator AddMassTransitTopicPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddMassTransitPublisher<T>(exchangeName, ExchangeType.Topic);

            return cfg;
        }

        public static IRabbitMqBusFactoryConfigurator AddMassTransitPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout)
            where T : class
        {
            cfg.Message<T>(x => x.SetEntityName(exchangeName));

            cfg.Publish<T>(x =>
            {
                x.AutoDelete = true;
                x.ExchangeType = exchangeType;
            });

            return cfg;
        }
    }
}
