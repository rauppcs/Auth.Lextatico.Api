using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Auth.Lextatico.Application.Dtos.Response;
using Auth.Lextatico.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Auth.Lextatico.Infra.CrossCutting.Middlewares
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
                {
                    exceptionHandlerApp.Run(async context =>
                    {
                        var response = new Response();
                        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                        var code = HttpStatusCode.InternalServerError;

                        if (exception is NotFoundException)
                        {
                            code = HttpStatusCode.NotFound;

                            response.AddError(exception.Message);
                        }
                        // else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
                        // else if (exception is MyException)             code = HttpStatusCode.BadRequest;
                        else
                            response.AddError("Ocorreu um erro inesperado.");

                        context.Response.ContentType = MediaTypeNames.Application.Json;

                        context.Response.StatusCode = (int)code;

                        await context.Response.WriteAsJsonAsync(response);
                    });
                });

            return app;
        }
    }
}
