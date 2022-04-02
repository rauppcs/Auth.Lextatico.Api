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

        protected readonly string SubscriptionName;

        public BaseConsumerDefinition(string exchangeName,
            string routingKey,
            string endpointName,
            string exchangeType = ExcgType.Direct,
            bool bindQueue = true)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            RoutingKey = routingKey;
            EndpointName = SubscriptionName = endpointName;
            _bindQueue = bindQueue;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<T> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;

            ConfigureRetry(endpointConfigurator);

            ConfigureCircuitBreaker(endpointConfigurator);

            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                if (_bindQueue) BindExchange(rmq);
            }
            else if (endpointConfigurator is IServiceBusReceiveEndpointConfigurator sb)
            {
                sb.Subscribe(ExchangeName, SubscriptionName);
            }
        }

        protected virtual void ConfigureRetry(IReceiveEndpointConfigurator rmq)
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

        protected virtual void ConfigureCircuitBreaker(IReceiveEndpointConfigurator rmq)
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
