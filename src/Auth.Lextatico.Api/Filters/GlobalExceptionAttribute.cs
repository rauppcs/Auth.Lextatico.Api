using System.Threading.Tasks;
using Auth.Lextatico.Application.Dtos.Response;
using Auth.Lextatico.Domain.Dtos.Message;
using Auth.Lextatico.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.Lextatico.Api.Filters
{
    /// <summary>
    /// Filter to catch exceptions during operations called by controller methods.
    /// </summary>
    public class GlobalExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Method called when an exception happens during an operation triggered by the controller.
        /// </summary>
        /// <param name="context">Context object for the exception.</param>
        public override void OnException(ExceptionContext context)
        {
            var response = new Response();

            var exception = context.Exception;

            if (exception is NotFoundException)
            {
                response.AddError(exception.Message);

                context.Result = new NotFoundObjectResult(response);

                return;
            }

            response.AddError("Ocorreu um erro inesperado.");

            var result = new ObjectResult(response);

            result.StatusCode = 500;

            context.Result = result;
        }

        /// <summary>
        /// Method called when an exception happens during an operation triggered by the controller.
        /// </summary>
        /// <param name="context">Context object for the exception.</param>
        /// <returns></returns>
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);

            return Task.CompletedTask;
        }
    }
}
