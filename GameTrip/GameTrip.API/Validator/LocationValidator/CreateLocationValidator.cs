using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(location => location)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(LocationMessage.LocationCanNotBeNull.Key)
            .WithMessage(LocationMessage.LocationCanNotBeNull.Message);

        #region Name
        RuleFor(location => location.Name)
            .NotNull()
            .NotEmpty()
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.NameCanNotBeNullOrEmpty.Key)
            .WithMessage(LocationMessage.NameCanNotBeNullOrEmpty.Message);
        #endregion

        #region Description
        RuleFor(location => location.Description)
            .NotNull()
            .NotEmpty()
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.NameCanNotBeNullOrEmpty.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeNullOrEmpty.Message);
        #endregion

        #region Latitude
        RuleFor(location => location.Latitude)
            .PrecisionScale(15, 12, true)
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.LatitudeIncorectPrecision.Key)
            .WithMessage(LocationMessage.LatitudeIncorectPrecision.Message);
        #endregion

        #region Longitude
        RuleFor(location => location.Longitude)
            .PrecisionScale(15, 12, true)
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.LongitudeIncorectPrecision.Key)
            .WithMessage(LocationMessage.LongitudeIncorectPrecision.Message);
        #endregion
    }
}