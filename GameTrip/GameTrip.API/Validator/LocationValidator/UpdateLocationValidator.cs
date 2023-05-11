using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LocationModels;

namespace GameTrip.API.Validator.LocationValidator;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationDto>
{
    public UpdateLocationValidator()
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
            .WithErrorCode(LocationMessage.DescriptionCanNotBeNullOrEmpty.Key)
            .WithMessage(LocationMessage.DescriptionCanNotBeNullOrEmpty.Message);
        #endregion

    }
}
