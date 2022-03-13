using System.Net.Mime;
using Auth.Lextatico.Api.Configurations;
using Auth.Lextatico.Api.Extensions;
using Auth.Lextatico.Infra.CrossCutting.Extensions;
using Auth.Lextatico.Infra.CrossCutting.IoC;
using Auth.Lextatico.Infra.Identity.User;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using HostEnvironmentEnvExtensions = Auth.Lextatico.Api.Extensions.HostEnvironmentEnvExtensions;

if (HostEnvironmentEnvExtensions.IsDocker())
    Thread.Sleep(30000);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostContext, builder) =>
{
    if (hostContext.HostingEnvironment.IsLocalDevelopment())
        builder.AddUserSecrets<Program>();
});

builder.Services
    .AddHttpContextAccessor()
    .AddLextaticoMessage()
    .AddLextaticoAspNetUser()
    .AddLextaticoEmailSettings(builder.Configuration)
    .AddLextaticoInfraServices()
    .AddLextaticoDomainServices()
    .AddLextaticoAutoMapper()
    .AddLextaticoApplicationServices()
    .AddLextaticoHealthChecks(builder.Configuration)
    .AddLextaticoContext(builder.Configuration)
    .AddLextaticoIdentity()
    .AddLextaticoJwtConfiguration(builder.Configuration)
    .AddLexitaticoCors()
    .AddLextaticoControllers()
    .AddLextaticoSwagger()
    .AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("doc/swagger.json", "Auth Lextatico Api v1"));

if (!app.Environment.IsProduction())
{
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

app.UseTransaction();

app.MapHealthChecks("/status",
              new HealthCheckOptions()
              {
                  ResponseWriter = async (context, report) =>
                  {
                      var result = JsonConvert.SerializeObject(
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
