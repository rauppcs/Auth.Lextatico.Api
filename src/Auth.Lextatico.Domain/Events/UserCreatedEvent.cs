using Auth.Lextatico.Domain.Models;

namespace Auth.Lextatico.Domain.Events
{
    public class UserCreatedEvent
    {
        public ApplicationUser ApplicationUser { get; set; }
    }
}
