using FluentValidation;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.Comment;

namespace GameTrip.API.Validator.CommentValidator;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(CommentMessage.UpdateCommentDtoNull.Key)
            .WithMessage(CommentMessage.UpdateCommentDtoNull.Message);

        RuleFor(dto => dto.CommentId)
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(CommentMessage.CommentIdNullOrEmpty.Key)
            .WithMessage(CommentMessage.CommentIdNullOrEmpty.Message);

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
