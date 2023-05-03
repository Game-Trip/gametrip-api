using FluentValidation;
using GameTrip.Domain.Errors;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class CreateLocationValidator : AbstractValidator<LocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(location => location)
            .Must(location => location is not null)
            .WithErrorCode(LocationErrors.LocationCanNotBeNull.Key)
            .WithMessage(LocationErrors.LocationCanNotBeNull.Message);

        RuleFor(location => location.Name)
            .Must(name => name is not null)
            .Unless(location => location is null)
            .WithErrorCode(LocationErrors.NameCanNotBeNull.Key)
            .WithMessage(LocationErrors.NameCanNotBeNull.Message);

        RuleFor(location => location.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .Unless(location => location is null)
            .Unless(location => location.Name is null)
            .WithErrorCode(LocationErrors.NameCanNotBeEmpty.Key)
            .WithMessage(LocationErrors.NameCanNotBeEmpty.Message);

        RuleFor(location => location.Description)
            .Must(name => name is not null)
            .Unless(location => location is null)
            .WithErrorCode(LocationErrors.NameCanNotBeNull.Key)
            .WithMessage(LocationErrors.DescriptionCanNotBeNull.Message);

        RuleFor(location => location.Description)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .Unless(location => location is null)
            .Unless(location => location.Description is null)
            .WithErrorCode(LocationErrors.NameCanNotBeEmpty.Key)
            .WithMessage(LocationErrors.DescriptionCanNotBeEmpty.Message);

        RuleFor(location => location.Latitude)
            .PrecisionScale(15, 12, true)
            .Unless(location => location is null)
            .WithErrorCode(LocationErrors.LatitudeIncorectPrecision.Key)
            .WithMessage(LocationErrors.LatitudeIncorectPrecision.Message);

        RuleFor(location => location.Longitude)
            .PrecisionScale(15, 12, true)
            .Unless(location => location is null)
            .WithErrorCode(LocationErrors.LongitudeIncorectPrecision.Key)
            .WithMessage(LocationErrors.LongitudeIncorectPrecision.Message);
    }
}