using System.Net.Mime;
using Auth.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions;
using Auth.Lextatico.Api.Configurations;
using Auth.Lextatico.Infra.CrossCutting.Extensions;
using Auth.Lextatico.Infra.CrossCutting.IoC;
using Auth.Lextatico.Infra.Identity.User;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HostEnvironmentEnvExtensions = Auth.Lextatico.Infra.CrossCutting.Extensions.HostEnvironmentEnvExtensions;
using Auth.Lextatico.Infra.CrossCutting.Middlewares;
using System.Text.Json;

if (HostEnvironmentEnvExtensions.IsDocker())
    Thread.Sleep(30000);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostContext, builder) =>
{
    if (hostContext.HostingEnvironment.IsLocalDevelopment())
        builder.AddUserSecrets<Program>();
});

builder.Host.UseLextaticoSerilog(builder.Environment, builder.Configuration);

builder.Services
    .AddHttpContextAccessor()
    .AddLextaticoMessage()
    .AddLextaticoAspNetUser()
    .AddLextaticoEmailSettings(builder.Configuration)
    .AddLextaticoUrlsConfiguration(builder.Configuration)
    .AddLextaticoInfraServices()
    .AddLextaticoDomainServices()
    .AddLextaticoAutoMapper()
    .AddLextaticoApplicationServices()
    .AddLextaticoHealthChecks(builder.Configuration, builder.Environment)
    .AddLextaticoContext(builder.Configuration, builder.Environment)
    .AddLextaticoIdentity()
    .AddLextaticoJwt(builder.Configuration)
    .AddLexitaticoCors()
    .AddLextaticoControllers()
    .AddLextaticoSwagger()
    .AddEndpointsApiExplorer();

if (!builder.Environment.IsProduction())
    builder.Services.AddLextaticoMassTransitWithRabbitMq(builder.Configuration);
else
    builder.Services.AddLextaticoMassTransitWithServiceBus(builder.Configuration);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("doc/swagger.json", "Auth Lextatico Api v1"));

if (!app.Environment.IsProduction())
{
    await app.Services.MigrateContextDbAsync();

    app.UseDeveloperExceptionPage();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();

    app.UseHsts();
}

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestSerilog();

app.UseErrorHandling();

app.UseLogging();

app.UseTransaction();

app.MapHealthChecks("/status",
              new HealthCheckOptions()
              {
                  ResponseWriter = async (context, report) =>
                  {
                      var result = JsonSerializer.Serialize(
                          new
                          {
                              statusApplication = report.Status.ToString(),
                              healthChecks = report.Entries.Select(e => new
                              {
                                  check = e.Key,
                                  ErrorMessage = e.Value.Exception?.Message,
                                  status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                              })
                          });
                      context.Response.ContentType = MediaTypeNames.Application.Json;
                      await context.Response.WriteAsync(result);
                  }
              });

app.MapHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
