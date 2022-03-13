using AutoMapper;
using Auth.Lextatico.Application.Dtos.User;
using Auth.Lextatico.Application.Services.Interfaces;
using Auth.Lextatico.Domain.Interfaces.Services;
using Auth.Lextatico.Domain.Security;

namespace Auth.Lextatico.Application.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthAppService(IMapper mapper,
            IAuthService authService,
            IUserService userService,
            ITokenService tokenService,
            TokenConfiguration tokenConfiguration)
        {
            _mapper = mapper;
            _authService = authService;
            _userService = userService;
            _tokenService = tokenService;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<AuthenticatedUserDto> LogInAsync(UserLogInDto userLogIn)
        {
            var result = await _authService.LogInAsync(userLogIn.Email, userLogIn.Password);

            if (result)
            {
                var userDto = _mapper.Map<UserDetailDto>(await _userService.GetUserByEmailAsync(userLogIn.Email));

                var (token, refreshToken) = _authService.GenerateFullJwt(userLogIn.Email);

                var authenticatedUser = new AuthenticatedUserDto(
                    userDto,
                    true,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddSeconds(_tokenConfiguration.Seconds),
                    token,
                    refreshToken,
                    DateTime.UtcNow.AddSeconds(_tokenConfiguration.SecondsRefresh));

                await _userService.UpdateRefreshTokenAsync(userLogIn.Email, authenticatedUser.RefreshToken, authenticatedUser.RefreshTokenExpiration);

                return authenticatedUser;
            }

            return null;
        }

        public async Task<AuthenticatedUserDto> RefreshTokenAsync(UserRefreshDto userRefresh)
        {
            var applicationUser = _userService.GetUserByRefreshToken(userRefresh.RefreshToken);

            if (applicationUser == null)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDetailDto>(await _userService.GetUserByEmailAsync(applicationUser.Email));

            var (token, refreshToken) = _authService.GenerateFullJwt(applicationUser.Email);

            var authenticatedUser = new AuthenticatedUserDto(
                userDto,
                true,
                DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(_tokenConfiguration.Seconds),
                token,
                refreshToken,
                DateTime.UtcNow.AddSeconds(_tokenConfiguration.SecondsRefresh));

            await _userService.UpdateRefreshTokenAsync(applicationUser.Email, authenticatedUser.RefreshToken, authenticatedUser.RefreshTokenExpiration);

            return authenticatedUser;
        }
    }
}
