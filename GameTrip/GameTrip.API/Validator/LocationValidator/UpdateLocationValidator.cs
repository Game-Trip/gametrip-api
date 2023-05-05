using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationDto>
{
    public UpdateLocationValidator()
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
            .WithErrorCode(LocationMessage.DescriptionCanNotBeNull.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeNull.Message);

        RuleFor(location => location.Description)
            .NotEmpty()
            .Unless(location => location is null)
            .Unless(location => location.Description is null)
            .WithErrorCode(LocationMessage.DescriptionCanNotBeEmpty.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeEmpty.Message);
        #endregion

    }
}
