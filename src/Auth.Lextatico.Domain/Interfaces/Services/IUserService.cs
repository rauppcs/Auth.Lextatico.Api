using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Lextatico.Domain.Models;

namespace Auth.Lextatico.Domain.Interfaces.Services
{
    public interface IUserService
    {
        
        Task<ApplicationUser> GetUserLoggedAsync();
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        ApplicationUser GetUserByRefreshToken(string refreshToken);
        Task<bool> CreateAsync(ApplicationUser applicationUser, string password);
        Task UpdateRefreshTokenAsync(string email, string refreshToken, DateTime refreshTokenExpiration);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string password, string resetToken);
    }
}
