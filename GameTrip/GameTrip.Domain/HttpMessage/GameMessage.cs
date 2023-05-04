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

    public static GameMessage NotFoundByName => new("GameNotFoundByName", "Game not found with provided Name");
    public static GameMessage AlreadyExist => new("GameAlreadyExist", "Game already exist");
    public static GameMessage Deleted => new("GameDeleted", "Game deleted");
    public static GameMessage Created => new("GameDeleted", "Game created");
}