using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LikeModels.Game;

namespace GameTrip.API.Validator.LikeValidator;

public class AddLikeGameValidator : AbstractValidator<AddLikeGameDto>
{
    public AddLikeGameValidator()
    {
        RuleFor(dto => dto)
           .NotNull().NotEmpty()
           .WithErrorCode(LikeMessage.AddLikeLocationDtoNull.Key)
           .WithMessage(LikeMessage.AddLikeLocationDtoNull.Message);

        #region Location
        RuleFor(dto => dto.GameId)
            .NotNull().NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.GameIdNullOrEmpty.Key)
            .WithMessage(LikeMessage.GameIdNullOrEmpty.Message);
        #endregion

        #region User
        RuleFor(dto => dto.UserId)
            .NotNull().NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.UserIdNullOrEmpty.Key)
            .WithMessage(LikeMessage.UserIdNullOrEmpty.Message);
        #endregion

        #region Value
        RuleFor(dto => dto.Value)
            .NotNull().NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.ValueNullOrEmpty.Key)
            .WithMessage(LikeMessage.ValueNullOrEmpty.Message);

        RuleFor(dto => dto.Value)
            .PrecisionScale(2, 1, true)
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.ValuePrecision.Key)
            .WithMessage(LikeMessage.ValuePrecision.Message);

        RuleFor(dto => dto.Value)
            .GreaterThanOrEqualTo(0)
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.ValueMoreOrEqualThan0.Key)
            .WithMessage(LikeMessage.ValueMoreOrEqualThan0.Message);

        RuleFor(dto => dto.Value)
            .LessThanOrEqualTo(5)
            .Unless(dto => dto is null)
            .WithErrorCode(LikeMessage.ValueLessOrEqualThan5.Key)
            .WithMessage(LikeMessage.ValueLessOrEqualThan5.Message);

        #endregion
    }
}
