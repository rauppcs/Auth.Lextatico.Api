using MassTransit;
using MassTransit.Serialization;
using MimeKit;
using ExcgType = RabbitMQ.Client.ExchangeType;

namespace Auth.Lextatico.Application.ConsumersDefinition
{
    public abstract class BaseConsumerDefinition<T> : ConsumerDefinition<T>
        where T : class, IConsumer
    {
        private readonly bool _bindQueue;

        protected readonly string ExchangeName;

        protected readonly string ExchangeType;

        protected readonly string RoutingKey;

        public BaseConsumerDefinition(string exchangeName,
            string routingKey,
            string endpointName,
            string exchangeType = ExcgType.Direct,
            bool bindQueue = true)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            RoutingKey = routingKey;
            EndpointName = endpointName;
            _bindQueue = bindQueue;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<T> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;
            if (!(endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq))
                return;

            // ConfigureDeserializer(rmq);
            ConfigureRetry(rmq);
            if (_bindQueue) BindExchange(rmq);
            ConfigureCircuitBreaker(rmq);
        }

        protected virtual void ConfigureDeserializer(IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.UseJsonDeserializer(true);
        }

        protected virtual void ConfigureRetry(IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.UseRetry(retry => { retry.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1)); });
        }

        protected virtual void BindExchange(IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind(ExchangeName, x =>
            {
                x.AutoDelete = true;
                x.RoutingKey = RoutingKey;
                x.ExchangeType = ExchangeType;
            });
        }

        protected virtual void ConfigureCircuitBreaker(IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.UseCircuitBreaker(cb =>
            {
                cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                cb.TripThreshold = 15;
                cb.ActiveThreshold = 10;
                cb.ResetInterval = TimeSpan.FromMinutes(5);
            });
        }
    }
}
