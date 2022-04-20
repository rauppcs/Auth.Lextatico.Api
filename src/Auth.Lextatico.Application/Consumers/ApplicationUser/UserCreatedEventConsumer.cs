using Auth.Lextatico.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using User = Auth.Lextatico.Domain.Models.ApplicationUser;

namespace Auth.Lextatico.Application.Consumers.ApplicationUser
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly UserManager<User> _userManager;

        public UserCreatedEventConsumer(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            await _userManager.CreateAsync(context.Message.ApplicationUser);
        }
    }
}
