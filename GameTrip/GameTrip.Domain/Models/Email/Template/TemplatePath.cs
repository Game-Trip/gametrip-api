using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Models.Email.Template;

public class TemplatePath : StringEnum
{
    public TemplatePath(string value) : base(value)
    {
    }

    public static TemplatePath Register => new("RegisterTemplate.html");
    public static TemplatePath ForgotPassword => new("FrogotPasswordTemplate.html");

    public override string ToString() => Value;
}