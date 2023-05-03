using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Errors;
public class TemplateErrors : StringEnumError
{
    public TemplateErrors(string key, string value) : base(key, value)
    {
    }

    public static TemplateErrors TemplateRegisterNotFound => new("TemplateRegisterNotFound", "Template file, Register not found");
    public static TemplateErrors TemplateFrogotPasswordNotFound => new("FrogotPassword", "Template file, FrogotPassword not found");
}
