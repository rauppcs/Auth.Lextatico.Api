#nullable enable

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

        public List<Notification> Errors { get; } = new List<Notification>();

        public List<Notification> Warnings { get; } = new List<Notification>();

        public void AddResult(object data) => Data = data;

        public void AddError(Notification error) => Errors.Add(error);

        public void AddError(string message) => Errors.Add(new Notification(string.Empty, message));

        public void AddError(string property, string message) => Errors.Add(new Notification(property, message));

        public void AddErrors(IList<Notification> errors) => Errors.AddRange(errors);

        public void AddWarning(Notification error) => Warnings.Add(error);

        public void AddWarning(string message) => Warnings.Add(new Notification(string.Empty, message));

        public void AddWarning(string property, string message) => Warnings.Add(new Notification(property, message));

        public void AddWarnings(IList<Notification> warnings) => Warnings.AddRange(warnings);
    }
}
