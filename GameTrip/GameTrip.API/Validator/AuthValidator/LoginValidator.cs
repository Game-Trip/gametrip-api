using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto)
            .NotNull().NotEmpty()
            .WithErrorCode(UserMessage.LoginDtoNull.Key)
            .WithMessage(UserMessage.LoginDtoNull.Message);

        #region Username
        RuleFor(dto => dto.Username)
            .NotNull()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.UsernameNull.Key)
            .WithMessage(UserMessage.UsernameNull.Message);

        RuleFor(dto => dto.Username)
            .NotEmpty()
            .Unless(dto => dto is null)
            .Unless(dto => dto.Username is null)
            .WithErrorCode(UserMessage.UsernameEmpty.Key)
            .WithMessage(UserMessage.UsernameEmpty.Message);
        #endregion

        #region Password

        RuleFor(dto => dto.Password)
            .NotNull()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordNull.Key)
            .WithMessage(UserMessage.PasswordNull.Message);

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .Unless(dto => dto is null)
            .Unless(dto => dto.Password is null)
            .WithErrorCode(UserMessage.PasswordEmpty.Key)
            .WithMessage(UserMessage.PasswordEmpty.Message);
        #endregion
    }
}
