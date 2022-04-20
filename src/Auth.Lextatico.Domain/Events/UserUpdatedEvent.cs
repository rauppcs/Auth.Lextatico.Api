using Auth.Lextatico.Domain.Models;

namespace Auth.Lextatico.Domain.Events
{
    public class UserUpdatedEvent
    {
        public ApplicationUser ApplicationUser { get; set; }
    }
}
