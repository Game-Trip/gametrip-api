using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;

public class GameMessage : StringEnumError
{
    public GameMessage(string key, string value) : base(key, value)
    {
    }

    #region Validator

    public static GameMessage GameCanNotBeNull => new("GameCanNotBeNull", "Game cannot be Null");
    public static GameMessage NameCanNotBeNull => new("GameNameCanNotBeNull", "Game name cannot be Null");
    public static GameMessage NameCanNotBeEmpty => new("GameNameCanNotBeEmpty", "Game name cannot be Empty");
    public static GameMessage DescriptionCanNotBeNull => new("GameDescriptionCanNotBeNull", "Game description cannot be Null");
    public static GameMessage DescriptionCanNotBeEmpty => new("GameDescriptionCanNotBeEmpty", "Game description cannot be Empty");
    public static GameMessage EditorCanNotBeNull => new("GameEditorCanNotBeNull", "Game editor cannot be Null");
    public static GameMessage EditorCanNotBeEmpty => new("GameEditorCanNotBeEmpty", "Game editor cannot be Empty");

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
}