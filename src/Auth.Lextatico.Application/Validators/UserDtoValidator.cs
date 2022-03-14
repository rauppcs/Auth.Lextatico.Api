using System.Linq;
using FluentValidation;
using Auth.Lextatico.Application.Dtos.User;
using Auth.Lextatico.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Lextatico.Application.Validators
{
    public abstract class UserDtoValidator<T> : AbstractValidator<T> where T : UserDto
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserDtoValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected void ValidateEmail()
        {
            RuleFor(userLoginDto => userLoginDto.Email)
                .EmailAddress()
                .WithMessage("Insira um endereço de email válido.");
        }

        protected void ValidatePassword()
        {
            RuleFor(userLoginDto => userLoginDto.Password)
                .Custom((password, context) =>
                {
                    var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

                    var result = passwordValidator.ValidateAsync(_userManager, null, password).Result;

                    if (!result.Succeeded)
                    {
                        var messages = result.Errors.Select(error => error.Description);

                        foreach (var message in messages)
                        {
                            context.AddFailure("", message);
                        }
                    }
                })
               .MustAsync(async (password, cancelationToken) =>
               {
                   var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

                   var result = await passwordValidator.ValidateAsync(_userManager, null, password);

                   return result.Succeeded;
               })
               .WithMessage("Senha informada não é válida.");
        }
    }
}
