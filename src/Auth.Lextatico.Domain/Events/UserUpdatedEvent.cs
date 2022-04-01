using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Lextatico.Domain.Models;

namespace Auth.Lextatico.Domain.Events
{
    public class UserUpdatedEvent
    {
        public ApplicationUser ApplicationUser { get; set; }
    }
}
