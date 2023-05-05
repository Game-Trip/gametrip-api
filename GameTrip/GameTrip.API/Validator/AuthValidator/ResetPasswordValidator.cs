using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(dto => dto)
            .NotNull().NotEmpty()
            .WithErrorCode(UserMessage.ResetPasswordDtoNull.Key)
            .WithMessage(UserMessage.ResetPasswordDtoNull.Message);

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
        RuleFor(dto => dto.PasswordConfirmation)
            .Must(passwordConfirmation => passwordConfirmation is not null)
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordConfirmationNull.Key)
            .WithMessage(UserMessage.PasswordConfirmationNull.Message);

        RuleFor(dto => dto.PasswordConfirmation)
            .Must(passwordConfirmation => !string.IsNullOrWhiteSpace(passwordConfirmation))
            .Unless(dto => dto is null)
            .Unless(dto => dto.PasswordConfirmation is null)
            .WithErrorCode(UserMessage.PasswordConfirmationEmpty.Key)
            .WithMessage(UserMessage.PasswordConfirmationEmpty.Message);

        RuleFor(dto => dto.PasswordConfirmation)
            .Equal(dto => dto.Password)
            .Unless(dto => dto is null)
            .Unless(dto => dto.Password is null
                           && dto.PasswordConfirmation is null
                           && !string.IsNullOrWhiteSpace(dto.Password)
                           && !string.IsNullOrWhiteSpace(dto.PasswordConfirmation))
            .WithErrorCode(UserMessage.PasswordConfirmationNotEqual.Key)
            .WithMessage(UserMessage.PasswordConfirmationNotEqual.Message);
        #endregion

        #region Token
        RuleFor(dto => dto.Token)
            .NotNull()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.TokenNull.Key)
            .WithMessage(UserMessage.TokenNull.Message);

        RuleFor(dto => dto.Token)
            .NotEmpty()
            .Unless(dto => dto is null)
            .Unless(dto => dto.Token is null)
            .WithErrorCode(UserMessage.TokenEmpty.Key)
            .WithMessage(UserMessage.TokenEmpty.Message);
        #endregion
    }
}
