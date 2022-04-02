using Auth.Lextatico.Application.Consumers.ApplicationUser;

namespace Auth.Lextatico.Application.ConsumersDefinition.ApplicationUser
{
    public class UserCreatedEventConsumerDefinition : BaseConsumerDefinition<UserCreatedEventConsumer>
    {
        public UserCreatedEventConsumerDefinition()
            : base("lextatico.exchange.UserCreatedEvent", "lextatico.UserCreated", "auth.lextatico.queue.UserCreatedEvent")
        {
        }
    }
}
