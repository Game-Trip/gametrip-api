namespace GameTrip.Domain.Models;

public abstract class StringEnum
{
    public string Value { get; private set; }

    protected StringEnum(string value) => Value = value;

    public override string ToString() => Value;
}