using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(UserMessage.RegisterDtoNull.Key)
            .WithMessage(UserMessage.RegisterDtoNull.Message);

        #region Username
        RuleFor(dto => dto.Username)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.UsernameNullOrEmpty.Key)
            .WithMessage(UserMessage.UsernameNullOrEmpty.Message);

        #endregion

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
        RuleFor(dto => dto.ConfirmPassword)
            .NotNull()
            .NotEmpty()
           .Unless(dto => dto is null)
           .WithErrorCode(UserMessage.PasswordConfirmationNullOrEmpty.Key)
           .WithMessage(UserMessage.PasswordConfirmationNullOrEmpty.Message);

        RuleFor(dto => dto.ConfirmPassword)
            .Equal(dto => dto.Password)
            .Unless(dto => dto is null)
            .Unless(dto => dto.Password is null && dto.ConfirmPassword is null && !string.IsNullOrWhiteSpace(dto.Password) && !string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            .WithErrorCode(UserMessage.PasswordConfirmationNotEqual.Key)
            .WithMessage(UserMessage.PasswordConfirmationNotEqual.Message);

        #endregion

    }
}
