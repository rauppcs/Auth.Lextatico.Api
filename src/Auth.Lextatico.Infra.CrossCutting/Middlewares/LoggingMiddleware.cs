using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Auth.Lextatico.Infra.CrossCutting.Middlewares
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<LoggingMiddleware>();

            return app;
        }
    }

    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var skipList = new List<string> { "swagger", "healthchecks-data-ui", "status" };

            var path = context.Request.Path.Value ?? "";

            if (skipList.Any(a => path.Contains(a)))
            {
                await _next(context);
            }
            else
            {
                await LogRequest(context);
                await LogResponse(context);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            var memoryStream = new MemoryStream();

            context.Request.EnableBuffering();

            await context.Request.Body.CopyToAsync(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var text = await new StreamReader(memoryStream).ReadToEndAsync();

            var headers = JsonSerializer.Serialize(context.Request.Headers);

            _logger.LogInformation("Http Request Information: \nSchema: {httpSchema} Host: {host} Path: {path} QueryString: {queryString} Headers: {headers} Request Body: {body}",
                context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString, headers, text);

            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        private async Task LogResponse(HttpContext context)
        {
            var memoryStream = new MemoryStream();

            var originalBody = context.Response.Body;

            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var text = await new StreamReader(memoryStream).ReadToEndAsync();

            memoryStream.Seek(0, SeekOrigin.Begin);

            var headers = JsonSerializer.Serialize(context.Request.Headers);

            _logger.LogInformation("Http Response Information: \nSchema: {httpSchema} Host: {host} Path: {path} QueryString: {queryString} Headers: {headers} Request Body: {body}",
                context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString, headers, text);

            await memoryStream.CopyToAsync(originalBody);
        }
    }
}
