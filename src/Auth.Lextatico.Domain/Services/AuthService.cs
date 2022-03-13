using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Lextatico.Domain.Configurations;
using Auth.Lextatico.Domain.Dtos.Message;
using Auth.Lextatico.Domain.Interfaces.Services;
using Auth.Lextatico.Domain.Models;
using Auth.Lextatico.Infra.Identity.User;
using Auth.Lextatico.Infra.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.Lextatico.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMessage _message;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(ITokenService tokenService,
                UserManager<ApplicationUser> userManger,
                SignInManager<ApplicationUser> signInManager,
                IMessage message)
        {
            _tokenService = tokenService;
            _userManager = userManger;
            _signInManager = signInManager;
            _message = message;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    _message.AddError(string.Empty, "Usuário bloqueado. Aguarde 5 minutos e tente novamente.");
                else if (result.IsNotAllowed)
                    _message.AddError(string.Empty, "Usuário não está liberado para fazer login.");
                else
                    _message.AddError(string.Empty, "Usuário ou senha incorreto.");
            }

            return result.Succeeded;
        }

        public (string token, string refreshToken) GenerateFullJwt(string email)
        {
            return _tokenService
                    .WithUserManager(_userManager)
                    .WithEmail(email)
                    .WithJwtClaims()
                    .WithUserClaims()
                    .WithUserRoles()
                    .BuildToken();
        }
    }
}
