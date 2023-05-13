using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Auth;

namespace GameTrip.API.Validator.AuthValidator;

public class UpdateGameTripUserValidator : AbstractValidator<UpdateGameTripUserDto>
{
    public UpdateGameTripUserValidator()
    {
        RuleFor(dto => dto)
        .NotNull()
        .NotEmpty()
        .WithErrorCode(UserMessage.UpdateGameTripUserDtoNull.Key)
        .WithMessage(UserMessage.UpdateGameTripUserDtoNull.Message);

        #region Username
        RuleFor(dto => dto.UserName)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.UsernameNullOrEmpty.Key)
            .WithMessage(UserMessage.UsernameNullOrEmpty.Message);

        #endregion

        #region Password

        RuleFor(dto => dto.Email)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(UserMessage.PasswordNullOrEmpty.Key)
            .WithMessage(UserMessage.PasswordNullOrEmpty.Message);

        RuleFor(dto => dto.Email)
            .EmailAddress()
            .Unless(dto => dto is null)
            .Unless(dto => dto.Email is null)
            .Unless(dto => !string.IsNullOrWhiteSpace(dto.Email))
            .WithErrorCode(UserMessage.PasswordNullOrEmpty.Key)
            .WithMessage(UserMessage.PasswordNullOrEmpty.Message);

        #endregion
    }
}
