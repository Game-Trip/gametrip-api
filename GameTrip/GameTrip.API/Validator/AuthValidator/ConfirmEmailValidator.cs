using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class ConfirmEmailValidator : AbstractValidator<ConfirmMailDto>
{
    public ConfirmEmailValidator()
    {
        RuleFor(dto => dto)
            .NotNull().NotEmpty()
            .WithErrorCode(UserMessage.ConfirmEmailDtoNull.Key)
            .WithMessage(UserMessage.ConfirmEmailDtoNull.Message);

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
