using Auth.Lextatico.Application.Dtos.User;

namespace Auth.Lextatico.Application.Services.Interfaces
{
    public interface IAuthAppService
    {
        Task<AuthenticatedUserDto> LogInAsync(UserLogInDto userLogIn);

        Task<AuthenticatedUserDto> RefreshTokenAsync(UserRefreshDto userRefresh);
    }
}
