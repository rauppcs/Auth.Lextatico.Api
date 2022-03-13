using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Auth.Lextatico.Infra.Data.Context;
using Auth.Lextatico.Domain.Models;
using Auth.Lextatico.Infra.Identity.Extensions;

namespace Auth.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureIdentity
    {
        public static IServiceCollection AddLextaticoIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddEntityFrameworkStores<LextaticoContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
