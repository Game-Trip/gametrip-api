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
    public static LocationMessage NameCanNotBeNull => new("LocationNameCanNotBeNull", "The Name field cannot be Null");
    public static LocationMessage NameCanNotBeEmpty => new("LocationNameCanNotBeEmpty", "The Name field cannot be Empty");
    public static LocationMessage DescriptionCanNotBeNull => new("LocationDescriptionCanNotBeNull", "The Description field cannot be Null");
    public static LocationMessage DescriptionCanNotBeEmpty => new("LocationDescriptionCanNotBeEmpty", "The Description field cannot be Empty");
    public static LocationMessage LatitudeIncorectPrecision => new("LatitudeIncorectPrecision", "The maximal precision for latitude field is decimal(15,12)");
    public static LocationMessage LongitudeIncorectPrecision => new("LongitudeIncorectPrecision", "The maximal precision for longitude field is decimal(15,12)");

    #endregion Validator

    public static LocationMessage AlreadyExistByName => new("LocationAlreadyExistByName", "The location with provided Name already exist");
    public static LocationMessage AlreadyExistByPos => new("LocationAlreadyExistByPosition", "The location with provided Position already exist");
    public static LocationMessage NotFoundByName => new("LocationNotFoundByName", "No location find with provided Name");
    public static LocationMessage NotFoundById => new("LocationNotFoundById", "No location find with provided Id");
    public static LocationMessage SuccesDeleted => new("LocationSuccesDeleted", "Location provided as been deleted");
    public static LocationMessage NotContainGameById => new("LocationNotContainLocationById", "Location provided do not contain Game provided with id");
    public static LocationMessage AlreadyContainGameById => new("LocationAlreadyContainLocationById", "Location provided already have Game provided with id");
}