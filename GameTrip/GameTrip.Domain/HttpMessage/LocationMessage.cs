using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;

public class LocationMessage : StringEnumError
{
    #region ctor

    public LocationMessage(string key, string value) : base(key, value)
    {
    }

    #endregion ctor

    #region Validator

    public static LocationMessage LocationCanNotBeNull => new("LocationCanNotBeNull", "Location cannot be Null");
    public static LocationMessage NameCanNotBeNullOrEmpty => new("LocationNameCanNotBeNullOrEmpty", "The Name field cannot be Null Or Empty");
    public static LocationMessage DescriptionCanNotBeNullOrEmpty => new("LocationDescriptionCanNotBeNullOrEmpty", "The Description field cannot be Null Or Empty");
    public static LocationMessage LatitudeIncorectPrecision => new("LatitudeIncorectPrecision", "The maximal precision for latitude field is decimal(15,12)");
    public static LocationMessage LongitudeIncorectPrecision => new("LongitudeIncorectPrecision", "The maximal precision for longitude field is decimal(15,12)");

    #endregion Validator

    public static LocationMessage AlreadyExistByName => new("LocationAlreadyExistByName", "The location with provided Name already exist");
    public static LocationMessage AlreadyExistByPos => new("LocationAlreadyExistByPosition", "The location with provided Position already exist");
    public static LocationMessage NotFoundByName => new("LocationNotFoundByName", "No location find with provided Name");
    public static LocationMessage NotFoundById => new("LocationNotFoundById", "No location find with provided Id");
    public static LocationMessage NotFoundByGameId => new("LocationNotFoundByGameId", "No locations found with the game of the provided GameId");
    public static LocationMessage NotFoundByGameName => new("LocationNotFoundByGameName", "No locations found with the game of the provided GameName");
    public static LocationMessage SuccesDeleted => new("LocationSuccesDeleted", "Location provided as been deleted");
    public static LocationMessage NotContainGameById => new("LocationNotContainLocationById", "Location provided do not contain Game provided with id");
    public static LocationMessage AlreadyContainGameById => new("LocationAlreadyContainLocationById", "Location provided already have Game provided with id");
    public static LocationMessage IdWithQueryAndDtoAreDifferent => new("LocationIdWithQueryAndDtoAreDifferent", "The provided locationId in route and provided locationId in Dto are not equals");
    public static LocationMessage UserNotAuthor => new("UserNotAuthor", "The user provided by Id is not the author of location");
    public static LocationMessage AlreadyValidate => new("AlreadyValidate", "The location already validated");
    public static LocationMessage AlreadyNotValidate => new("AlreadyUnValidate", "The location already not validated");
    public static LocationMessage NowValidate => new("NowValidate", "The location is now validate");
    public static LocationMessage NowNotValidate => new("NowNotValidate", "The location is now not validate");
    public static LocationMessage LocationUpdateRequestSuccess => new("LocationUpdateRequestSuccess", "The request to update location has been registered, it will soon be processed by the administrator");
    public static LocationMessage NotFoundUpdateRequest => new("NotFoundUpdateRequest", "This location haven't update request");
}