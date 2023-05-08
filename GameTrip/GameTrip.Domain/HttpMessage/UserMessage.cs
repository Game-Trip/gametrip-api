using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
/// <summary>
/// The user message.
/// </summary>

public class UserMessage : StringEnumError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMessage"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    public UserMessage(string value, string key) : base(value, key)
    {
    }

    #region Validator
    public static UserMessage LoginDtoNull => new("LoginDtoNull", "The LoginDto can not be Null");
    public static UserMessage RegisterDtoNull => new("RegisterDtoNull", "The RegisterDto can not be Null");
    public static UserMessage ConfirmEmailDtoNull => new("ConfirmEmailDtoNull", "The ConfirmEmailDto can not be Null");
    public static UserMessage ForgotPasswordDtoNull => new("ForgotPasswordDtoNull", "The ForgotPasswordDto can not be Null");
    public static UserMessage ResetPasswordDtoNull => new("ResetPasswordDtoNull", "The ResetPasswordDto can not be Null");
    public static UserMessage UsernameNull => new("UserUserNameNull", "The UserName field can not be Null");
    public static UserMessage UsernameEmpty => new("UserUserNameEmtpy", "The UserName field can not be Empty");
    public static UserMessage EmailNull => new("UserEmailNull", "The Email field can not be Null");
    public static UserMessage EmailEmtpy => new("UserEmailEmtpy", "The Email field can not be Empty");
    public static UserMessage EmailDoEmail => new("UserEmailDoEmail", "The Email field must be a valid Email");
    public static UserMessage PasswordNull => new("UserPasswordNull", "The Password field can not be Null");
    public static UserMessage PasswordEmpty => new("UserPasswordEmpty", "The Password field can not be Emtpy");
    public static UserMessage PasswordConfirmationNull => new("UserPasswordConfirmationNull", "The PasswordConfirmation field can not be Null");
    public static UserMessage PasswordConfirmationEmpty => new("UserPasswordConfirmationEmpty", "The PasswordConfirmation field can not be Empty");
    public static UserMessage PasswordConfirmationNotEqual => new("UserPasswordConfirmationNotEqual", "The PasswordConfirmation field and Password field must be Equals");
    public static UserMessage TokenNull => new("UserTokenNull", "The Token field can not be Null");
    public static UserMessage TokenEmpty => new("UserTokenEmpty", "The Token field can not be Empty");

    #endregion

    public static UserMessage FailedLogin => new("FailedLogin", "The login or the password is invalid");
    public static UserMessage NotFoundById => new("NotFound", "No user found with provided ID");
    public static UserMessage NotFoundByUserName => new("NotFound", "No user found with provided UserName");
    public static UserMessage NotFoundByMail => new("NotFound", "No user found with provided Email");
    public static UserMessage MailAlreadyExist => new("MailAlreadyExist", "An account already exists with the Mail provided");
    public static UserMessage UserNameAlreadyExist => new("UserNameAlreadyExist", "An account already exists with the UserName provided");
    public static UserMessage NeverLikeAnyLocation => new("NeverLikeAnyLocation", "The User provided never likes any location");
    public static UserMessage NeverLikeAnyGame => new("NeverLikeAnyGame", "The User provided never likes any games");
}