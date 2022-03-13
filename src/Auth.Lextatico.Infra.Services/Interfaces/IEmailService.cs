using System.Threading.Tasks;
using Auth.Lextatico.Infra.Services.Models.EmailService;

namespace Auth.Lextatico.Infra.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
