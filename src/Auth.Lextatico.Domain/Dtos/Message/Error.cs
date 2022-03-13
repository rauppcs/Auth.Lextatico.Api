namespace Auth.Lextatico.Domain.Dtos.Message
{
    public class Error
    {
        public Error(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; set; }
        public string Message { get; set; }
    }
}
