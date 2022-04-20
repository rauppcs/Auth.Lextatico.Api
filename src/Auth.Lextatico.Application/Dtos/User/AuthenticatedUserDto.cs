namespace Auth.Lextatico.Application.Dtos.User
{
    public class AuthenticatedUserDto
    {
        public AuthenticatedUserDto(UserDetailDto user, bool authenticated, DateTime created, DateTime expiration, string accessToken, string refreshToken, DateTime refreshTokenExpiration)
        {
            User = user;
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
        }

        public UserDetailDto User { get; set; }
        public bool Authenticated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
