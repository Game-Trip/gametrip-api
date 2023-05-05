using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(dto => dto)
            .NotNull().NotEmpty()
            .WithErrorCode(UserMessage.ForgotPasswordDtoNull.Key)
            .WithMessage(UserMessage.ForgotPasswordDtoNull.Message);

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
    }
}
