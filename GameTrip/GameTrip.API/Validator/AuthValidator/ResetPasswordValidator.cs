using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(UserMessage.ResetPasswordDtoNull.Key)
            .WithMessage(UserMessage.ResetPasswordDtoNull.Message);

        #region Email
        RuleFor(dto => dto.Email)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.EmailNullOrEmpty.Key)
            .WithMessage(UserMessage.EmailNullOrEmpty.Message);

        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithErrorCode(UserMessage.EmailDoEmail.Key)
            .WithMessage(UserMessage.EmailDoEmail.Message);
        #endregion

        #region Password
        RuleFor(dto => dto.Password)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordNullOrEmpty.Key)
            .WithMessage(UserMessage.PasswordNullOrEmpty.Message);

        #endregion

        #region ConfirmPassword
        RuleFor(dto => dto.PasswordConfirmation)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordConfirmationNullOrEmpty.Key)
            .WithMessage(UserMessage.PasswordConfirmationNullOrEmpty.Message);

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
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.TokenNullOrEmpty.Key)
            .WithMessage(UserMessage.TokenNullOrEmpty.Message);

        #endregion
    }
}
