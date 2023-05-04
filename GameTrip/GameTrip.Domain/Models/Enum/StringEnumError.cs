namespace GameTrip.Domain.Models.Enum;

public abstract class StringEnumError
{
    public string Message { get; private set; }
    public string Key { get; private set; }

    protected StringEnumError(string key, string value)
    {
        Message = value;
        Key = key;
    }

    public override string ToString() => Message;
    public string Format(params object?[] args) => string.Format(Message, args);
}