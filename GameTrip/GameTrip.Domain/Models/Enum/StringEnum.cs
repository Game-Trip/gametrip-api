namespace GameTrip.Domain.Models.Enum;

public abstract class StringEnum
{
    public string Value { get; private set; }

    protected StringEnum(string value) => Value = value;

    public override string ToString() => Value;
}