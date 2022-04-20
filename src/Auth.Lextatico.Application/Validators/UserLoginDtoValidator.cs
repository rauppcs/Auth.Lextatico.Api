using Auth.Lextatico.Application.Dtos.User;
using Auth.Lextatico.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Lextatico.Application.Validators
{
    public class UserLoginDtoValidator : UserDtoValidator<UserLogInDto>
    {
        public UserLoginDtoValidator(UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            ValidateEmail();
        }
    }
}
