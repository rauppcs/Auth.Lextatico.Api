using Auth.Lextatico.Application.Dtos.Filter;
using Auth.Lextatico.Application.Dtos.Response;
using Auth.Lextatico.Application.Helpers;
using Auth.Lextatico.Domain.Dtos.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Lextatico.Api.Controllers
{
    /// <summary>
    /// Base controller, containing the validation of the response object and the appropriate status returns of the possible application status codes.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class LextaticoController : ControllerBase
    {
        private readonly IMessage _message;

        protected LextaticoController(IMessage message)
        {
            _message = message;
        }

        private bool ValidResponse()
        {
            if (_message is null || !_message.IsValid())
                return false;

            return true;
        }

        private Response MountResponse<T>(T? data, PaginationFilterDto? pagination, int total)
        {
            var isPagination = pagination != null;

            Response response;

            if (isPagination)
                response = Pagination.CreatePagedReponse(data, pagination, total);
            else
                response = new Response(data);

            response.AddErrors(_message.Errors);

            response.AddWarnings(_message.Warnings);

            return response;
        }

        private Response MountResponse()
        {
            return MountResponse(new { }, null, 0);
        }

        protected virtual IActionResult ReturnOk()
        {
            if (!ValidResponse())
            {
                var response = MountResponse();

                return ReturnBadRequest(response);
            }

            return NoContent();
        }

        protected virtual IActionResult ReturnOk<T>(T data)
        {
            return ReturnOk(data, null, 0);
        }

        protected virtual IActionResult ReturnOk<T>(T? data, PaginationFilterDto? pagination, int total)
        {
            var response = MountResponse(data, pagination, total);

            if (!ValidResponse())
                return ReturnBadRequest(response);

            return Ok(response);
        }

        protected virtual IActionResult ReturnCreated<T>(T data, PaginationFilterDto? pagination = null, int total = 0)
        {
            var response = MountResponse(data, pagination, total);

            if (!ValidResponse())
                return ReturnBadRequest(response);

            return Created(_message.GetLocation(), response);
        }

        protected virtual IActionResult ReturnCreated()
        {
            if (!ValidResponse())
            {
                var response = MountResponse();

                return ReturnBadRequest(response);
            }

            return Created(_message.GetLocation());
        }

        protected virtual IActionResult ReturnAccepted<T>(T data, PaginationFilterDto pagination = null, int total = 0)
        {
            var response = MountResponse(data, pagination, total);

            if (!ValidResponse())
                return ReturnBadRequest(response);

            return Accepted(response);
        }

        protected virtual IActionResult ReturnAccepted()
        {
            if (!ValidResponse())
                return BadRequest();

            return Accepted();
        }

        protected virtual IActionResult ReturnBadRequest(Response response)
        {
            return BadRequest(response);
        }

        protected virtual IActionResult ReturnFileResult(string nameFile, string file, string contentType)
        {
            var ms = new MemoryStream();
            var streamWriter = new StreamWriter(ms);

            streamWriter.WriteLine(file);
            streamWriter.Flush();

            ms.Position = 0;

            return File(ms, contentType, nameFile);
        }

        protected virtual async Task<IActionResult> ReturnFileResultAsync(string nameFile, string file, string contentType)
        {
            var ms = new MemoryStream();
            var streamWriter = new StreamWriter(ms);

            await streamWriter.WriteLineAsync(file);
            await streamWriter.FlushAsync();

            ms.Position = 0;

            return File(ms, contentType, nameFile);
        }

        protected virtual IActionResult ReturnCustomResult(IActionResult actionResult)
        {
            if (!ValidResponse())
                return BadRequest();

            return actionResult;
        }

        private IActionResult Created(string location)
        {
            var result = new StatusCodeResult(StatusCodes.Status201Created);

            HttpContext.Response.Headers["Location"] = location;

            return result;
        }
    }
}
