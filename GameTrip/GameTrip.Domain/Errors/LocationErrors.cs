using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Errors;
public class LocationErrors : StringEnumError
{
    #region ctor
    public LocationErrors(string key, string value) : base(key, value)
    {
    }
    #endregion

    #region Validator
    public static LocationErrors LocationCanNotBeNull => new("LocationCanNotBeNull", "Location cannot be Null");
    public static LocationErrors NameCanNotBeNull => new("LocationNameCanNotBeNull", "The Name field cannot be Null");
    public static LocationErrors NameCanNotBeEmpty => new("LocationNameCanNotBeEmpty", "The Name field cannot be Empty");
    public static LocationErrors DescriptionCanNotBeNull => new("LocationDescriptionCanNotBeNull", "The Description field cannot be Null");
    public static LocationErrors DescriptionCanNotBeEmpty => new("LocationDescriptionCanNotBeEmpty", "The Description field cannot be Empty");
    public static LocationErrors LatitudeIncorectPrecision => new("LatitudeIncorectPrecision", "The maximal precision for latitude field is decimal(15,12)");
    public static LocationErrors LongitudeIncorectPrecision => new("LongitudeIncorectPrecision", "The maximal precision for longitude field is decimal(15,12)");
    #endregion

    public static LocationErrors AlreadyExistByName => new("LocationAlreadyExistByName", "The location with provided Name already exist");
    public static LocationErrors AlreadyExistByPos => new("LocationAlreadyExistByPosition", "The location with provided Position already exist");
    public static LocationErrors NotFoundByName => new("LocationNotFoundByName", "No location find with provided Name");
    public static LocationErrors NotFoundById => new("LocationNotFoundById", "No location find with provided Id");

}
