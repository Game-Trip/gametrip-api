using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(UserMessage.LoginDtoNull.Key)
            .WithMessage(UserMessage.LoginDtoNull.Message);

        #region Username
        RuleFor(dto => dto.Username)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.UsernameNullOrEmpty.Key)
            .WithMessage(UserMessage.UsernameNullOrEmpty.Message);

        #endregion

        #region Password

        RuleFor(dto => dto.Password)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordNullOrEmpty.Key)
            .WithMessage(UserMessage.PasswordNullOrEmpty.Message);

        #endregion
    }
}
