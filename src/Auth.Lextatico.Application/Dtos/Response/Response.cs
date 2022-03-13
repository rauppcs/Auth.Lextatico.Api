#nullable enable

using System.Collections.Generic;
using System.Linq;
using Auth.Lextatico.Domain.Dtos.Message;

namespace Auth.Lextatico.Application.Dtos.Response
{
    public class Response
    {
        public Response()
        {
        }
        public Response(object? data)
        {
            Data = data;
        }

        public object? Data { get; private set; }
        public IList<Error> Errors { get; } = new List<Error>();

        public void AddResult(object data) => Data = data;

        public void AddError(Error error) => Errors.Add(error);

        public void AddError(string message) => Errors.Add(new Error(string.Empty, message));

        public void AddError(string property, string message) => Errors.Add(new Error(property, message));
    }
}
