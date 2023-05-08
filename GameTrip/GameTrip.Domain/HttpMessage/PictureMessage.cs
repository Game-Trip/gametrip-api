using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
public class PictureMessage : StringEnumError
{
    public PictureMessage(string key, string value) : base(key, value)
    { }

    #region Validator
    public static PictureMessage AddPictureToLocationDto => new("AddPictureToLocationDto", "The AddPictureToLocationDto can not be Null or Empty");
    public static PictureMessage NameNotNullOrEmpty => new("NameNotNullOrEmpty", "The Name filed can not be Null or Empty");
    public static PictureMessage DescriptionNotNullOrEmpty => new("DescriptionNotNullOrEmpty", "The Description filed can not be Null or Empty");
    public static PictureMessage LocationIdNotNullOrEmpty => new("LocationIdNotNullOrEmpty", "The LocationId filed can not be Null or Empty");
    #endregion
}
