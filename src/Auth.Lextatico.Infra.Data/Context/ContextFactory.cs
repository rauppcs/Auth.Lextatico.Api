using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Auth.Lextatico.Infra.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<LextaticoContext>
    {
        /// <summary>
        /// Used for migrations
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns a valid DbContext.</returns>
        public LextaticoContext CreateDbContext(string[] args)
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory())
                .GetDirectories("Lextatico.Api").FirstOrDefault().FullName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.LocalDevelopment.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.Load("Lextatico.Infra.Data"))
                .Build();

            var sqlStringBuilder = new SqlConnectionStringBuilder(builder.GetConnectionString(nameof(LextaticoContext)))
            {
                Password = builder["DbPassword"]
            };

            var connectionString = sqlStringBuilder.ToString();

            var optionsBuilder = new DbContextOptionsBuilder<LextaticoContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new LextaticoContext(optionsBuilder.Options);
        }
    }
}
