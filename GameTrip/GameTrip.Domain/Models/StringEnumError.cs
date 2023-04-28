namespace GameTrip.Domain.Models;

public abstract class StringEnumError
{
    public string Value { get; private set; }
    public string Key { get; private set; }

    protected StringEnumError(string key, string value)
    {
        Value = value;
        Key = key;
    }

    public override string ToString() => Value;
}