using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.API.Validator.PictureValidator;
public class AddPictureToGameValidator : AbstractValidator<AddPictureToGameDto>
{
    public AddPictureToGameValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(PictureMessage.AddPictureToGameDto.Key)
            .WithMessage(PictureMessage.AddPictureToGameDto.Message);

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

        RuleFor(dto => dto.GameId)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(PictureMessage.GameIdNotNullOrEmpty.Key)
            .WithMessage(PictureMessage.GameIdNotNullOrEmpty.Message);
    }
}
