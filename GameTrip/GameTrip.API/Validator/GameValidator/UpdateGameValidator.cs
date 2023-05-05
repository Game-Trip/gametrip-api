using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.API.Validator.GameValidator;

public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
{
    public UpdateGameValidator()
    {
        RuleFor(game => game)
            .NotNull().NotEmpty()
            .WithErrorCode(GameMessage.GameCanNotBeNull.Key)
            .WithMessage(GameMessage.GameCanNotBeNull.Message);

        #region Name
        RuleFor(game => game.Name)
            .NotNull()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.NameCanNotBeNull.Key)
            .WithMessage(GameMessage.NameCanNotBeNull.Message);

        RuleFor(game => game.Name)
            .NotEmpty()
            .Unless(game => game is null)
            .Unless(game => game.Name is null)
            .WithErrorCode(GameMessage.NameCanNotBeEmpty.Key)
            .WithMessage(GameMessage.NameCanNotBeEmpty.Message);
        #endregion

        #region Description
        RuleFor(game => game.Description)
            .NotNull()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.DescriptionCanNotBeNull.Key)
            .WithMessage(GameMessage.DescriptionCanNotBeNull.Message);

        RuleFor(game => game.Description)
            .NotEmpty()
            .Unless(game => game is null)
            .Unless(game => game.Description is null)
            .WithErrorCode(GameMessage.DescriptionCanNotBeEmpty.Key)
            .WithMessage(GameMessage.DescriptionCanNotBeEmpty.Message);
        #endregion

        #region Editor
        RuleFor(game => game.Editor)
            .NotNull()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.EditorCanNotBeNull.Key)
            .WithMessage(GameMessage.EditorCanNotBeNull.Message);

        RuleFor(game => game.Editor)
            .NotEmpty()
            .Unless(game => game is null)
            .Unless(game => game.Editor is null)
            .WithErrorCode(GameMessage.EditorCanNotBeEmpty.Key)
            .WithMessage(GameMessage.EditorCanNotBeEmpty.Message);
        #endregion

        #region ReleaseDate
        RuleFor(game => game.ReleaseDate)
            .NotNull()
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.ReleaseCanNotBeNull.Key)
            .WithMessage(GameMessage.ReleaseCanNotBeNull.Message);
        #endregion
    }
}