using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
public class CommentMessage : StringEnumError
{
    public CommentMessage(string key, string message) : base(key, message)
    {
    }

    public static CommentMessage AddCommentToLocationDtoNull => new("AddCommentToLocationDtoNull", "The AddCommentToLocationDto can not be null");
    public static CommentMessage UpdateCommentDtoNull => new("UpdateCommentDtoNull", "The UpdateCommentDto can not be null");
    public static CommentMessage TextNullOrEmpty => new("TextNullOrEmpty", "The field Text can not be Null or Empty");
    public static CommentMessage CommentIdNullOrEmpty => new("CommentIdNullOrEmpty", "The field CommentId can not be Null or Empty");
    public static CommentMessage NotFoundById => new("NotFoundById", "No Comment found with the provied Id");
    public static CommentMessage LocationIdNullOrEmpty => new("LocationIdNullOrEmpty", "The field LocationId can not be Null or Empty");
    public static CommentMessage UserIdNullOrEmpty => new("UserIdNullOrEmpty", "The field UserId can not be Null or Empty");
    public static CommentMessage SuccessCreate => new("SucessCreate", "The provided comment have been created");
    public static CommentMessage LocationIdAndIdInDtoNotEqual => new("LocationIdAndIdInDtoNotEqual", "The provided LocationId are not equals in Route param and Dto");
    public static CommentMessage UserNotAuthor => new("UserNotAuthor", "The user provided by Id is not the author of message");
    public static CommentMessage AlreadyValidate => new("AlreadyValidate", "The message already validated");
    public static CommentMessage AlreadyNotValidate => new("AlreadyUnValidate", "The message already not validated");
    public static CommentMessage NowValidate => new("NowValidate", "The message is now validate");
    public static CommentMessage NowNotValidate => new("NowNotValidate", "The message is now not validate");
}
