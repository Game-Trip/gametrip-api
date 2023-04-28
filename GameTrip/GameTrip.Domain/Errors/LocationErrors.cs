using GameTrip.Domain.Models;

namespace GameTrip.Domain.Errors;
public class LocationErrors : StringEnumError
{
    public LocationErrors(string key, string value) : base(key, value)
    {
    }

    public static LocationErrors AlreadyExist => new("AlreadyExist", "The provided location already exist");
    public static LocationErrors NotFoundByName => new("NotFoundByName", "No location find with provided Name");
    public static LocationErrors NotFoundById => new("NotFoundById", "No location find with provided Id");

}
