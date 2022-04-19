using System;
using System.Threading.Tasks;
using Auth.Lextatico.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Lextatico.Infra.CrossCutting.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static async Task<IServiceProvider> MigrateContextDbAsync(this IServiceProvider serviceProvider)
        {
            using var escope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();

            var contextDb = escope.ServiceProvider.GetRequiredService<LextaticoContext>();

            if (!(contextDb is null))
            {
                await contextDb.Database.MigrateAsync();
            }

            return serviceProvider;
        }
    }
}
