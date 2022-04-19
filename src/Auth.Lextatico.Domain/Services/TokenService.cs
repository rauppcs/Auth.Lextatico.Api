using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Auth.Lextatico.Domain.Interfaces.Services;
using Auth.Lextatico.Domain.Models;
using Auth.Lextatico.Domain.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Lextatico.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;
        private TokenConfiguration _tokenConfiguration;
        private SigningConfiguration _signingConfiguration;
        private ApplicationUser _applicationUser;
        private ICollection<Claim> _userClaims;
        private ICollection<Claim> _jwtClaims;
        private ClaimsIdentity _identityClaims;

        public TokenService(IHttpContextAccessor httpContextAccessor, TokenConfiguration tokenConfiguration, SigningConfiguration signingConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenConfiguration = tokenConfiguration;
            _signingConfiguration = signingConfiguration;
        }

        public ITokenService WithUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            return this;
        }

        public ITokenService WithTokenConfiguration(TokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration ?? throw new ArgumentException(nameof(tokenConfiguration));
            return this;
        }

        public ITokenService WithSigningConfiguration(SigningConfiguration signingConfiguration)
        {
            _signingConfiguration = signingConfiguration ?? throw new ArgumentException(nameof(signingConfiguration));
            return this;
        }

        public ITokenService WithEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException(nameof(email));

            _applicationUser = _userManager.FindByEmailAsync(email).Result;
            _userClaims = new List<Claim>();
            _jwtClaims = new List<Claim>();
            _identityClaims = new ClaimsIdentity();

            return this;
        }

        public ITokenService WithJwtClaims()
        {
            _jwtClaims.Add(new Claim("sub", _applicationUser.Id.ToString()));
            _jwtClaims.Add(new Claim("email", _applicationUser.Email));
            _jwtClaims.Add(new Claim("name", _applicationUser.Name));
            _jwtClaims.Add(new Claim("username", _applicationUser.UserName));
            _jwtClaims.Add(new Claim("jti", Guid.NewGuid().ToString()));
            _jwtClaims.Add(new Claim("nbf", ToUnixEpochDate(DateTime.UtcNow).ToString()));
            _jwtClaims.Add(new Claim("iat", ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            _identityClaims.AddClaims(_jwtClaims);

            return this;
        }

        public ITokenService WithUserClaims()
        {
            _userClaims = _userManager.GetClaimsAsync(_applicationUser).Result;

            _identityClaims.AddClaims(_userClaims);

            return this;
        }

        public ITokenService WithUserRoles()
        {
            var userRoles = _userManager.GetRolesAsync(_applicationUser).Result;

            userRoles.ToList().ForEach(r => _identityClaims.AddClaim(new Claim(ClaimTypes.Role, r)));

            return this;
        }

        public (string token, string refreshToken) BuildToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                Subject = _identityClaims,
                Expires = DateTime.UtcNow.AddSeconds(_tokenConfiguration.Seconds),
                SigningCredentials = _signingConfiguration.SigningCredentials
            });

            var token = tokenHandler.WriteToken(securityToken);

            var refreshToken = CreateRefreshToken();

            return (token, refreshToken);
        }

        private ClaimsPrincipal GetClaimsPrincipal()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var token = header.Split(" ")[1];

            return GetClaimsPrincipal(token);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = GetValidationParameters();

            var claims = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            return claims;
        }

        public string GetEmail() => GetClaimsPrincipal().FindFirst(JwtRegisteredClaimNames.Email).Value;

        public string GetUserName() => GetClaimsPrincipal().FindFirst("username").Value;

        public string GetEmail(string token) => GetClaimsPrincipal(token).FindFirst(JwtRegisteredClaimNames.Email).Value;

        public string GetUserName(string token) => GetClaimsPrincipal(token).FindFirst("username").Value;

        private string CreateRefreshToken()
        {
            var key = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss}{Guid.NewGuid().ToString()}";

            using var md5 = System.Security.Cryptography.MD5.Create();

            var keyBytes = System.Text.Encoding.ASCII.GetBytes(key);
            var hashBytes = md5.ComputeHash(keyBytes);

            var builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("X2"));
            }

            return builder.ToString();
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);

        public TokenValidationParameters GetValidationParameters()
            => new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _tokenConfiguration.Issuer,
                ValidAudience = _tokenConfiguration.Audience,
                IssuerSigningKey = _signingConfiguration.Key
            };
    }
}
