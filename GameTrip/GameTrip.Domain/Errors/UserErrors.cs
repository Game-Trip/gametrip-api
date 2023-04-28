using GameTrip.Domain.Models;

namespace GameTrip.Domain.Errors;

public class UserError : StringEnumError
{
    public UserError(string value, string key) : base(value, key)
    {
    }

    public static UserError FailedLogin => new("FailedLogin", "The login or the password is invalid");
    public static UserError NotFoundById => new("NotFound", "No user found with provided ID");
    public static UserError NotFoundByUserName => new("NotFound", "No user found with provided UserName");
    public static UserError NotFoundByMail => new("NotFound", "No user found with provided Email");
    public static UserError MailAlreadyExist => new("MailAlreadyExist", "An account already exists with the Mail provided");
    public static UserError UserNameAlreadyExist => new("UserNameAlreadyExist", "An account already exists with the UserName provided");
}