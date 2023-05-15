using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Comment;

namespace GameTrip.API.Validator.CommentValidator;

public class AddCommentToLocationValidator : AbstractValidator<AddCommentToLocationDto>
{
    public AddCommentToLocationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(CommentMessage.AddCommentToLocationDtoNull.Key)
            .WithMessage(CommentMessage.AddCommentToLocationDtoNull.Message);

        RuleFor(dto => dto.Text)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(CommentMessage.TextNullOrEmpty.Key)
            .WithMessage(CommentMessage.TextNullOrEmpty.Message);

        RuleFor(dto => dto.UserId)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(CommentMessage.UserIdNullOrEmpty.Key)
            .WithMessage(CommentMessage.UserIdNullOrEmpty.Message);

        RuleFor(dto => dto.LocationId)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(CommentMessage.LocationIdNullOrEmpty.Key)
            .WithMessage(CommentMessage.LocationIdNullOrEmpty.Message);
    }
}
