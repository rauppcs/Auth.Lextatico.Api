using FluentValidation;
using Auth.Lextatico.Application.Dtos.User;

namespace Auth.Lextatico.Application.Validators
{
    public class UserRefreshDtoValidator : AbstractValidator<UserRefreshDto>
    {
        public UserRefreshDtoValidator()
        {
            ValidateRefreshToken();
        }

        public void ValidateRefreshToken()
        {
            RuleFor(userRefresh => userRefresh.RefreshToken)
                .NotEmpty()
                .WithMessage("RefreshToken deve ser informado");
        }
    }
}
