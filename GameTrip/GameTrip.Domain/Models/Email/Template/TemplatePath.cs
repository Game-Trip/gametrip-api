namespace GameTrip.Domain.Models.Email.Template;

public class TemplatePath
{
    private TemplatePath(string value)
    { Value = value; }

    public string Value { get; private set; }

    public static TemplatePath Register => new("RegisterTemplate.html");

    public override string ToString()
    {
        return Value;
    }
}