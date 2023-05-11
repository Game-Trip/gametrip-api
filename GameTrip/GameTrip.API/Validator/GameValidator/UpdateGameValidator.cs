using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.API.Validator.GameValidator;

public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
{
    public UpdateGameValidator()
    {
        RuleFor(game => game)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(GameMessage.GameCanNotBeNull.Key)
            .WithMessage(GameMessage.GameCanNotBeNull.Message);

        #region Name
        RuleFor(game => game.Name)
            .NotNull()
            .NotEmpty()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.NameCanNotBeNullOrEmpty.Key)
            .WithMessage(GameMessage.NameCanNotBeNullOrEmpty.Message);

        #endregion

        #region Description
        RuleFor(game => game.Description)
            .NotNull()
            .NotEmpty()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.DescriptionCanNotBeNullOrEmpty.Key)
            .WithMessage(GameMessage.DescriptionCanNotBeNullOrEmpty.Message);

        #endregion

        #region Editor
        RuleFor(game => game.Editor)
            .NotNull()
            .NotEmpty()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.EditorCanNotBeNullOrEmpty.Key)
            .WithMessage(GameMessage.EditorCanNotBeNullOrEmpty.Message);
        #endregion

        #region ReleaseDate
        RuleFor(game => game.ReleaseDate)
            .NotNull()
            .NotEmpty()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.ReleaseCanNotBeNullOrEmpty.Key)
            .WithMessage(GameMessage.ReleaseCanNotBeNullOrEmpty.Message);
        #endregion
    }
}