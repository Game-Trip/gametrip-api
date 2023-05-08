using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
public class LikeMessage : StringEnumError
{
    public LikeMessage(string key, string value) : base(key, value)
    {
    }

    #region Validator
    public static LikeMessage AddLikeLocationDtoNull => new("AddLikeLocationDtoNull", "AddLikeLocationDto can not be Null");
    public static LikeMessage LocationIdNullOrEmpty => new("LocationIdNullOrEmpty", "The LocationId field can not be Null or Empty");
    public static LikeMessage GameIdNullOrEmpty => new("GameIdNullOrEmpty", "The GameId field can not be Null or Empty");
    public static LikeMessage UserIdNullOrEmpty => new("UserIdNullOrEmpty", "The UserId field can not be Null or Empty");
    public static LikeMessage ValueNullOrEmpty => new("ValueNull", "The Value field can not be Null or Empty");
    public static LikeMessage ValuePrecision => new("ValuePrecision", "The Value field must have a precision of 1 digit before and after the decimal point");
    public static LikeMessage ValueMoreOrEqualThan0 => new("ValueMoreOrEqualThan0", "The Value field must be greater or equal than zero");
    public static LikeMessage ValueLessOrEqualThan5 => new("ValueLessOrEqualThan5", "The Value field must be less or equal than five");

    #endregion

    public static LikeMessage UserAlreadyLikeLocation => new("UserAlreadyLikeLocation", "The user already liked the location provided");

}
