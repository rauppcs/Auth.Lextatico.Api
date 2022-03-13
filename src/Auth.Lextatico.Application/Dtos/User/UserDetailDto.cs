using System;

namespace Auth.Lextatico.Application.Dtos.User
{
    public class UserDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
