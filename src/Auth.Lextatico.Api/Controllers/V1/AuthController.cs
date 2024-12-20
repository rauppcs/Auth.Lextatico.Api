using Asp.Versioning;
using Auth.Lextatico.Application.Dtos.User;
using Auth.Lextatico.Application.Services.Interfaces;
using Auth.Lextatico.Domain.Dtos.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Lextatico.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AuthController : LextaticoController
    {
        private readonly IAuthAppService _authAppService;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IMessage message, IAuthAppService authAppService)
            : base(message)
        {
            _logger = logger;
            _authAppService = authAppService;
        }
        
        [HttpPost, Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogInDto userLogin)
        {
            _logger.LogInformation("Start login, user {userEmail}.", userLogin.Email);

            var result = await _authAppService.LogInAsync(userLogin);

            _logger.LogInformation("Finish login, user {userEmail}.", userLogin.Email);

            return ReturnOk(result);
        }

        [HttpPost, Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] UserRefreshDto userRefresh)
        {
            var result = await _authAppService.RefreshTokenAsync(userRefresh);

            return ReturnOk(result);
        }

        [HttpGet, Route("[action]")]
        public IActionResult ValidateToken()
        {
            return ReturnOk();
        }
    }
}
