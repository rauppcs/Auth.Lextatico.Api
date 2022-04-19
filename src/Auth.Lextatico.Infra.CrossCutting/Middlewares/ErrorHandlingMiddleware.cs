using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Auth.Lextatico.Application.Dtos.Response;
using Auth.Lextatico.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Auth.Lextatico.Infra.CrossCutting.Middlewares
{
    public static class ErrorHandlingExtension
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<ErrorHandlingMiddleware>();

            return app;
        }
    }

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            var response = new Response();

            logger.LogError(exception, "Erro na aplicação");

            var code = HttpStatusCode.InternalServerError;

            if (exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;

                response.AddError(exception.Message);
            }
            // else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            // else if (exception is MyException)             code = HttpStatusCode.BadRequest;
            else
            {
                code = HttpStatusCode.InternalServerError;

                response.AddError("Ocorreu um erro inesperado.");
            }

            var result = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
