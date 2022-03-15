namespace Auth.Lextatico.Domain.Dtos.Message
{
    public class Message : IMessage
    {
        public IList<Notification> Errors { get; set; } = new List<Notification>();

        public IList<Notification> Warnings { get; set; } = new List<Notification>();

        private string _locationObjectCreated = string.Empty;

        public void AddError(Notification error) => Errors.Add(error);

        public void AddError(string property, string message) => Errors.Add(new Notification(property, message));

        public void AddError(string message) => Errors.Add(new Notification(string.Empty, message));

        public void AddWarning(Notification error) => Warnings.Add(error);

        public void AddWarning(string message) => Warnings.Add(new Notification(string.Empty, message));

        public void AddWarning(string property, string message) => Warnings.Add(new Notification(property, message));

        public bool IsValid() => !Errors.Any();

        public void ClearErrors() => Errors.Clear();

        public string GetLocation() => _locationObjectCreated;

        public void SetLocation(string location) => _locationObjectCreated = location;
    }

    public interface IMessage
    {
        IList<Notification> Errors { get; set; }
        void AddError(Notification error);
        void AddError(string property, string message);
        void AddError(string message);
        IList<Notification> Warnings { get; set; }
        void AddWarning(Notification warning);
        void AddWarning(string property, string message);
        void AddWarning(string message);
        bool IsValid();
        void ClearErrors();
        string GetLocation();
        void SetLocation(string location);
    }
}
