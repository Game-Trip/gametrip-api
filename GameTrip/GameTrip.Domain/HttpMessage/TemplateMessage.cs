using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Errors;

public class TemplateMessage : StringEnumError
{
    public TemplateMessage(string key, string value) : base(key, value)
    {
    }

    public static TemplateMessage TemplateRegisterNotFound => new("TemplateRegisterNotFound", "Template file, Register not found");
    public static TemplateMessage TemplateFrogotPasswordNotFound => new("FrogotPassword", "Template file, FrogotPassword not found");
}