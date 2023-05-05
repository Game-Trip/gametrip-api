using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(dto => dto)
            .NotNull().NotEmpty()
            .WithErrorCode(UserMessage.RegisterDtoNull.Key)
            .WithMessage(UserMessage.RegisterDtoNull.Message);

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

        #region Email
        RuleFor(dto => dto.Email)
            .NotNull()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.EmailNull.Key)
            .WithMessage(UserMessage.EmailNull.Message);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .Unless(dto => dto is null)
            .Unless(dto => dto.Email is null)
            .WithErrorCode(UserMessage.EmailEmtpy.Key)
            .WithMessage(UserMessage.EmailEmtpy.Message);

        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithErrorCode(UserMessage.EmailDoEmail.Key)
            .WithMessage(UserMessage.EmailDoEmail.Message);
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

        #region ConfirmPassword
        RuleFor(dto => dto.ConfirmPassword)
            .NotNull()
           .Unless(dto => dto is null)
           .WithErrorCode(UserMessage.PasswordConfirmationNull.Key)
           .WithMessage(UserMessage.PasswordConfirmationNull.Message);

        RuleFor(dto => dto.ConfirmPassword)
            .NotEmpty()
            .Unless(dto => dto is null)
            .Unless(dto => dto.ConfirmPassword is null)
            .WithErrorCode(UserMessage.PasswordConfirmationEmpty.Key)
            .WithMessage(UserMessage.PasswordConfirmationEmpty.Message);

        RuleFor(dto => dto.ConfirmPassword)
            .Equal(dto => dto.Password)
            .Unless(dto => dto is null)
            .Unless(dto => dto.Password is null && dto.ConfirmPassword is null && !string.IsNullOrWhiteSpace(dto.Password) && !string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            .WithErrorCode(UserMessage.PasswordConfirmationNotEqual.Key)
            .WithMessage(UserMessage.PasswordConfirmationNotEqual.Message);

        #endregion

    }
}
