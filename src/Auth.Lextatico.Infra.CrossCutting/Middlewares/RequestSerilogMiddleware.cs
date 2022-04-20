using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Auth.Lextatico.Infra.Identity.User;
using Microsoft.AspNetCore.Builder;

namespace Auth.Lextatico.Infra.CrossCutting.Middlewares
{
    public static class RequestSerilogExtensions
    {
        public static IApplicationBuilder UseRequestSerilog(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<RequestSerilogMiddleware>();

            return app;
        }
    }

    public class RequestSerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestSerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("userId", context?.User?.GetUserId() ?? ""))
            using (LogContext.PushProperty("userName", context?.User?.GetUserName() ?? ""))
            using (LogContext.PushProperty("userEmail", context?.User?.GetUserEmail() ?? ""))
            {
                return _next.Invoke(context);
            }
        }
    }
}
