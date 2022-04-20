namespace Auth.Lextatico.Infra.Services.Models.EmailService
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
