using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;

namespace GameTrip.API.Validator.GameValidator;

public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
{
    public UpdateGameValidator()
    {
        RuleFor(game => game)
            .Must(game => game is not null)
            .WithErrorCode(GameMessage.GameCanNotBeNull.Key)
            .WithMessage(GameMessage.GameCanNotBeNull.Message);

        RuleFor(game => game.Name)
            .Must(name => name is not null)
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.NameCanNotBeNull.Key)
            .WithMessage(GameMessage.NameCanNotBeNull.Message);

        RuleFor(game => game.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .Unless(game => game is null)
            .Unless(game => game.Name is null)
            .WithErrorCode(GameMessage.NameCanNotBeEmpty.Key)
            .WithMessage(GameMessage.NameCanNotBeEmpty.Message);

        RuleFor(game => game.Description)
            .Must(desc => desc is not null)
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.DescriptionCanNotBeNull.Key)
            .WithMessage(GameMessage.DescriptionCanNotBeNull.Message);

        RuleFor(game => game.Description)
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .Unless(game => game is null)
            .Unless(game => game.Description is null)
            .WithErrorCode(GameMessage.DescriptionCanNotBeEmpty.Key)
            .WithMessage(GameMessage.DescriptionCanNotBeEmpty.Message);

        RuleFor(game => game.Editor)
            .Must(editor => editor is not null)
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.EditorCanNotBeNull.Key)
            .WithMessage(GameMessage.EditorCanNotBeNull.Message);

        RuleFor(game => game.Editor)
            .Must(editor => !string.IsNullOrWhiteSpace(editor))
            .Unless(game => game is null)
            .Unless(game => game.Editor is null)
            .WithErrorCode(GameMessage.EditorCanNotBeEmpty.Key)
            .WithMessage(GameMessage.EditorCanNotBeEmpty.Message);

        RuleFor(game => game.ReleaseDate)
            .Must(release => release is not null)
            .Unless(game => game is null)
            .WithErrorCode(GameMessage.ReleaseCanNotBeNull.Key)
            .WithMessage(GameMessage.ReleaseCanNotBeNull.Message);
    }
}