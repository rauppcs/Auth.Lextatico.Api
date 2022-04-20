using Microsoft.AspNetCore.Identity;

namespace Auth.Lextatico.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
    }
}
