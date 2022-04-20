namespace Auth.Lextatico.Domain.Models
{
    public class RefreshToken : Base
    {
        public RefreshToken()
        {
        }
        public RefreshToken(string token, DateTime tokenExpiration, Guid applicationUserId, ApplicationUser applicationUser)
        {
            Token = token;
            TokenExpiration = tokenExpiration;
            ApplicationUserId = applicationUserId;
            ApplicationUser = applicationUser;
        }

        public string Token { get; private set; }
        public DateTime TokenExpiration { get; private set; }
        public Guid ApplicationUserId { get; private set; }
        public virtual ApplicationUser ApplicationUser { get; private set; }
    }
}
