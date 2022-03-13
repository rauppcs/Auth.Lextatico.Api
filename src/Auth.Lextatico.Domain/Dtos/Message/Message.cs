namespace Auth.Lextatico.Domain.Dtos.Message
{
    public class Message : IMessage
    {
        public IList<Error> Errors { get; set; } = new List<Error>();
        private string _locationObjectCreated = string.Empty;

        public void AddError(Error error) => Errors.Add(error);

        public void AddError(string property, string message) => Errors.Add(new Error(property, message));

        public void AddError(string message) => Errors.Add(new Error(string.Empty, message));

        public bool IsValid() => !Errors.Any();

        public void ClearErrors() => Errors.Clear();

        public string GetLocation() => _locationObjectCreated;

        public void SetLocation(string location) => _locationObjectCreated = location;
    }

    public interface IMessage
    {
        IList<Error> Errors { get; set; }
        void AddError(Error error);
        void AddError(string property, string message);
        void AddError(string message);
        bool IsValid();
        void ClearErrors();
        string GetLocation();
        void SetLocation(string location);
    }
}
