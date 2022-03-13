using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Lextatico.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> LogInAsync(string email, string password);

        (string token, string refreshToken) GenerateFullJwt(string email);
    }
}
