using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class ConfirmEmailValidator : AbstractValidator<ConfirmMailDto>
{
    public ConfirmEmailValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(UserMessage.ConfirmEmailDtoNull.Key)
            .WithMessage(UserMessage.ConfirmEmailDtoNull.Message);

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
