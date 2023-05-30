using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;

public class GameMessage : StringEnumError
{
    public GameMessage(string key, string value) : base(key, value)
    {
    }

    #region Validator

    public static GameMessage GameCanNotBeNull => new("GameCanNotBeNull", "Game cannot be Null");
    public static GameMessage NameCanNotBeNullOrEmpty => new("GameNameCanNotBeNullOrEmpty", "Game name cannot be Null Or Empty");
    public static GameMessage DescriptionCanNotBeNullOrEmpty => new("GameDescriptionCanNotBeNullOrEmpty", "Game description cannot be Null Or Empty");
    public static GameMessage EditorCanNotBeNullOrEmpty => new("GameEditorCanNotBeNullOrEmpty", "Game editor cannot be Null Or Empty");
    public static GameMessage ReleaseCanNotBeNullOrEmpty => new("GameReleaseCanNotBeNullOrEmpty", "Game release cannot be Null Or Empty");

    #endregion Validator

    public static GameMessage NotFoundById => new("GameNotFoundById", "Game not found with provided Id");
    public static GameMessage NotFoundByLocationId => new("GameNotFoundByLocationId", "No games not found with the location of the provided location Id");
    public static GameMessage NotFoundByName => new("GameNotFoundByName", "Game not found with provided Name");
    public static GameMessage NotFoundByLocationName => new("GameNotFoundByLocationName", "No games not found with the location of the provided location Name");
    public static GameMessage AlreadyExist => new("GameAlreadyExist", "Game already exist");
    public static GameMessage SuccesDeleted => new("GameDeleted", "Game deleted");
    public static GameMessage SuccesCreated => new("GameDeleted", "Game created");
    public static GameMessage AddedToLocation => new("GameAddedToLocation", "The provided Game has been added to provided location");
    public static GameMessage RemovedToLocation => new("GameRemovedToLocation", "The provided Game has been removed to provided location");
    public static GameMessage IdWithQueryAndDtoAreDifferent => new("GameIdWithQueryAndDtoAreDifferent", "The provided gameId in route and provided gameId in Dto are not equals");
}