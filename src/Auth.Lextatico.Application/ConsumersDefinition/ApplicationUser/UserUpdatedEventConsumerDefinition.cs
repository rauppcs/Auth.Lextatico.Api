using Auth.Lextatico.Application.Consumers.ApplicationUser;

namespace Auth.Lextatico.Application.ConsumersDefinition.ApplicationUser
{
    public class UserUpdatedEventConsumerDefinition : BaseConsumerDefinition<UserUpdatedEventConsumer>
    {
        public UserUpdatedEventConsumerDefinition()
            : base("lextatico.exchange:UserUpdatedEvent", "lextatico.UserUpdated", "auth.lextatico.queue.UserUpdatedEvent")
        {
        }
    }
}
