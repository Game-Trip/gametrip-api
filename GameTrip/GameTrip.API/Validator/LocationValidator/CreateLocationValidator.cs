using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(location => location)
            .NotNull().NotEmpty()
            .WithErrorCode(LocationMessage.LocationCanNotBeNull.Key)
            .WithMessage(LocationMessage.LocationCanNotBeNull.Message);

        #region Name
        RuleFor(location => location.Name)
            .NotNull()
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.NameCanNotBeNull.Key)
            .WithMessage(LocationMessage.NameCanNotBeNull.Message);

        RuleFor(location => location.Name)
            .NotEmpty()
            .Unless(location => location is null)
            .Unless(location => location.Name is null)
            .WithErrorCode(LocationMessage.NameCanNotBeEmpty.Key)
            .WithMessage(LocationMessage.NameCanNotBeEmpty.Message);
        #endregion

        #region Description
        RuleFor(location => location.Description)
            .NotNull()
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.NameCanNotBeNull.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeNull.Message);

        RuleFor(location => location.Description)
            .NotEmpty()
            .Unless(location => location is null)
            .Unless(location => location.Description is null)
            .WithErrorCode(LocationMessage.NameCanNotBeEmpty.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeEmpty.Message);
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