using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationDto>
{
    public UpdateLocationValidator()
    {
        RuleFor(location => location)
            .Must(location => location is not null)
            .WithErrorCode(LocationMessage.LocationCanNotBeNull.Key)
            .WithMessage(LocationMessage.LocationCanNotBeNull.Message);

        RuleFor(location => location.Name)
            .Must(name => name is not null)
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.NameCanNotBeNull.Key)
            .WithMessage(LocationMessage.NameCanNotBeNull.Message);

        RuleFor(location => location.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .Unless(location => location is null)
            .Unless(location => location.Name is null)
            .WithErrorCode(LocationMessage.NameCanNotBeEmpty.Key)
            .WithMessage(LocationMessage.NameCanNotBeEmpty.Message);

        RuleFor(location => location.Description)
            .Must(desc => desc is not null)
            .Unless(location => location is null)
            .WithErrorCode(LocationMessage.DescriptionCanNotBeNull.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeNull.Message);

        RuleFor(location => location.Description)
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Unless(location => location is null)
            .Unless(location => location.Description is null)
            .WithErrorCode(LocationMessage.DescriptionCanNotBeEmpty.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeEmpty.Message);



    }
}
