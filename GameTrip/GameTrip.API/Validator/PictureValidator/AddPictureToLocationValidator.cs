using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.API.Validator.PictureValidator;

public class AddPictureToLocationValidator : AbstractValidator<AddPictureToLocationDto>
{
    public AddPictureToLocationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(PictureMessage.AddPictureToLocationDto.Key)
            .WithMessage(PictureMessage.AddPictureToLocationDto.Message);

        RuleFor(dto => dto.Name)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(PictureMessage.NameNotNullOrEmpty.Key)
            .WithMessage(PictureMessage.NameNotNullOrEmpty.Message);

        RuleFor(dto => dto.Description)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(PictureMessage.DescriptionNotNullOrEmpty.Key)
            .WithMessage(PictureMessage.DescriptionNotNullOrEmpty.Message);

        RuleFor(dto => dto.LocationId)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(PictureMessage.LocationIdNotNullOrEmpty.Key)
            .WithMessage(PictureMessage.LocationIdNotNullOrEmpty.Message);
    }
}
