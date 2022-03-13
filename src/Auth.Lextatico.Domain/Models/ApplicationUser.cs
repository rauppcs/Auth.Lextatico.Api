using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Auth.Lextatico.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; private set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
    }
}
