using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
public class PictureMessage : StringEnumError
{
    public PictureMessage(string key, string value) : base(key, value)
    { }

    #region Validator
    public static PictureMessage AddPictureToLocationDto => new("AddPictureToLocationDto", "The AddPictureToLocationDto can not be Null or Empty");
    public static PictureMessage AddPictureToGameDto => new("AddPictureToGameDto", "The AddPictureToGameDto can not be Null or Empty");
    public static PictureMessage NameNotNullOrEmpty => new("NameNotNullOrEmpty", "The Name filed can not be Null or Empty");
    public static PictureMessage DescriptionNotNullOrEmpty => new("DescriptionNotNullOrEmpty", "The Description filed can not be Null or Empty");
    public static PictureMessage LocationIdNotNullOrEmpty => new("LocationIdNotNullOrEmpty", "The LocationId filed can not be Null or Empty");
    public static PictureMessage GameIdNotNullOrEmpty => new("GameIdNotNullOrEmpty", "The GameId filed can not be Null or Empty");
    #endregion

    public static PictureMessage SucessAddToLocation => new("SucessAddToGame", "The picture has been added to Location provided");
    public static PictureMessage SucessAddToGame => new("SucessAddToGame", "The picture has been added to Game provided");
    public static PictureMessage DeleteSucces => new("DeleteSucces", "Picture has been Deleted");
    public static PictureMessage NotFoundById => new("PictureNotFoundById", "No Picture found with provided Id");
    public static PictureMessage UserNotAuthor => new("UserNotAuthor", "The user provided by Id is not the author of picture");
    public static PictureMessage AlreadyValidate => new("AlreadyValidate", "The picture already validated");
    public static PictureMessage AlreadyNotValidate => new("AlreadyUnValidate", "The picture already not validated");
    public static PictureMessage NowValidate => new("NowValidate", "The picture is now validate");
    public static PictureMessage NowNotValidate => new("NowNotValidate", "The picture is now not validate");

}
