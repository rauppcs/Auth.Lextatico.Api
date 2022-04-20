using Auth.Lextatico.Domain.Configurations;
using Auth.Lextatico.Domain.Dtos.Message;
using Auth.Lextatico.Domain.Interfaces.Services;
using Auth.Lextatico.Domain.Models;
using Auth.Lextatico.Infra.Identity.User;
using Auth.Lextatico.Infra.Services.Interfaces;
using Auth.Lextatico.Infra.Services.Models.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.Lextatico.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMessage _message;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAspNetUser _aspNetUser;
        private readonly Urls _urls;
        public UserService(ITokenService tokenService, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager, IAspNetUser aspNetUser, IMessage message, IOptions<Urls> urls, IEmailService emailService)
        {
            _tokenService = tokenService;
            _userManager = userManger;
            _signInManager = signInManager;
            _aspNetUser = aspNetUser;
            _message = message;
            _urls = urls.Value;
            _emailService = emailService;
        }

        

        public async Task<ApplicationUser> GetUserLoggedAsync()
        {
            var email = _aspNetUser.GetUserEmail();

            var applicationUser = await GetUserByEmailAsync(email);

            if (applicationUser == null)
                _message.AddError(string.Empty, "Usuário não encontrado");

            return applicationUser;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);

            if (applicationUser == null)
                _message.AddError(string.Empty, "Usuário não encontrado");

            return applicationUser;
        }

        public ApplicationUser GetUserByRefreshToken(string refreshToken)
        {
            var applicationUser = _userManager.Users.FirstOrDefault(user => user.RefreshTokens
                    .Any(refresh => refresh.Token == refreshToken
                        && DateTime.UtcNow <= refresh.TokenExpiration));

            if (applicationUser == null)
                _message.AddError(string.Empty, "Token ou RefreshToken inválido, faça o login novamente.");

            return applicationUser;
        }

        public async Task<bool> CreateAsync(ApplicationUser applicationUser, string password)
        {
            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _message.AddError(string.Empty, error.Description);
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> SignInAsync(string email, string password)
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

        public async Task UpdateRefreshTokenAsync(string email, string refreshToken, DateTime refreshTokenExpiration)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);

            var refreshTokenModel = new RefreshToken(refreshToken, refreshTokenExpiration, applicationUser.Id, applicationUser);

            applicationUser.RefreshTokens.Add(refreshTokenModel);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            var link = $"{_urls.LextaticoFront}/resetPassword?resetToken={resetToken}&email={applicationUser.Email}";

            var resetEmailRequest = new EmailRequest
            {
                Name = applicationUser.Name,
                Email = applicationUser.Email,
                Subject = "Reset password",
                Body = $"Agora falta pouco, para resetar sua senha clique <a target=\"_blank\" href=\"{link}\">aqui</a>."
            };

            await _emailService.SendEmailAsync(resetEmailRequest);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string password, string resetToken)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);

            var result = await _userManager.ResetPasswordAsync(applicationUser, resetToken, password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _message.AddError(string.Empty, error.Description);
                }
            }

            return result.Succeeded;
        }
    }
}
