using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Lextatico.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using User = Auth.Lextatico.Domain.Models.ApplicationUser;

namespace Auth.Lextatico.Application.Consumers.ApplicationUser
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly UserManager<User> _userManager;

        public UserUpdatedEventConsumer(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var user = context.Message.ApplicationUser;
            var userDb = await _userManager.FindByIdAsync(context.Message.ApplicationUser.Id.ToString());

            userDb.Name = user.Name;
            userDb.UserName = user.UserName;
            userDb.Email = user.Email;
            userDb.PasswordHash = user.PasswordHash;
            userDb.EmailConfirmed = user.EmailConfirmed;
            userDb.SecurityStamp = user.SecurityStamp;
            userDb.ConcurrencyStamp = user.ConcurrencyStamp;

            await _userManager.UpdateAsync(userDb);
        }
    }
}
